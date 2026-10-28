using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Bff.Models;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Services
{
    public class BffService : IScopedService
    {
        private const string SessionCookieName = "bff_session_id";

        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTimeService _dateTimeService;
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BffService(
            ApplicationDbContext dbContext,
            IDateTimeService dateTimeService,
            IOptionsMonitor<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _dateTimeService = dateTimeService;
            _appSettings = appSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SetTokensAsync(TokenResponse tokenResponse, CancellationToken cancellationToken = default)
        {
            await ClearTokensAsync(cancellationToken);

            DateTimeOffset utcNow = _dateTimeService.UtcNow;
            DateTimeOffset accessTokenExpiresAt = utcNow.AddSeconds(tokenResponse.ExpiresIn);
            bool hasRefreshToken = !string.IsNullOrWhiteSpace(tokenResponse.RefreshToken);

            DateTimeOffset sessionExpiresAt = hasRefreshToken
                ? utcNow.AddSeconds(_appSettings.CurrentValue.AbsoluteRefreshTokenLifetimeSeconds)
                : accessTokenExpiresAt;

            var session = new BffSession
            {
                Id = Guid.NewGuid(),
                AccessToken = tokenResponse.AccessToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshToken = hasRefreshToken ? tokenResponse.RefreshToken : null,
                SessionExpiresAt = sessionExpiresAt
            };

            _dbContext.BffSessions.Add(session);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            if (hasRefreshToken)
            {
                cookieOptions.MaxAge = sessionExpiresAt - utcNow;
            }

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                SessionCookieName,
                session.Id.ToString(),
                cookieOptions);
        }

        public async Task<Tokens> GetTokensAsync(CancellationToken cancellationToken = default)
        {
            Guid? sessionId = ReadSessionIdFromCookie();

            if (sessionId == null)
            {
                return null;
            }

            BffSession session = await _dbContext.BffSessions
                .FirstOrDefaultAsync(x => x.Id == sessionId.Value, cancellationToken);

            if (session == null)
            {
                DeleteSessionCookie();
                return null;
            }

            if (session.SessionExpiresAt <= _dateTimeService.UtcNow)
            {
                session.Deleted = true;
                _dbContext.Update(session);
                await _dbContext.SaveChangesAsync(cancellationToken);

                DeleteSessionCookie();
                return null;
            }

            return new Tokens
            {
                AccessToken = session.AccessToken,
                ExpiresAt = session.AccessTokenExpiresAt,
                RefreshToken = session.RefreshToken
            };
        }

        public async Task UpdateTokensAsync(TokenResponse tokenResponse, CancellationToken cancellationToken = default)
        {
            Guid? sessionId = ReadSessionIdFromCookie();

            if (!sessionId.HasValue)
            {
                return;
            }

            BffSession session = await _dbContext.BffSessions
                .FirstOrDefaultAsync(x => x.Id == sessionId.Value, cancellationToken);

            if (session == null)
            {
                return;
            }

            session.AccessToken = tokenResponse.AccessToken;
            session.AccessTokenExpiresAt = _dateTimeService.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            if (!string.IsNullOrWhiteSpace(tokenResponse.RefreshToken))
            {
                session.RefreshToken = tokenResponse.RefreshToken;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ClearTokensAsync(CancellationToken cancellationToken = default)
        {
            Guid? sessionId = ReadSessionIdFromCookie();

            if (!sessionId.HasValue)
            {
                return;
            }

            BffSession session = await _dbContext.BffSessions
                .FirstOrDefaultAsync(x => x.Id == sessionId.Value, cancellationToken);

            if (session != null)
            {
                session.Deleted = true;
                _dbContext.Update(session);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            DeleteSessionCookie();
        }

        private Guid? ReadSessionIdFromCookie()
        {
            string sessionId = _httpContextAccessor.HttpContext.Request.Cookies[SessionCookieName];

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return null;
            }

            if (!Guid.TryParse(sessionId, out Guid parsedSessionId))
            {
                DeleteSessionCookie();
                return null;
            }

            return parsedSessionId;
        }

        private void DeleteSessionCookie()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(SessionCookieName);
        }
    }
}
