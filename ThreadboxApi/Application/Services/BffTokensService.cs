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
    public class BffTokensService : IScopedService
    {
        private const string SessionCookieName = "bff_session_id";

        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTimeService _dateTimeService;
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BffTokensService(
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
            DateTimeOffset sessionExpiresAt = utcNow.AddSeconds(_appSettings.CurrentValue.AbsoluteRefreshTokenLifetimeSeconds);

            var session = new BffSession
            {
                Id = Guid.NewGuid(),
                AccessToken = tokenResponse.AccessToken,
                AccessTokenExpiresAt = utcNow.AddSeconds(tokenResponse.ExpiresIn),
                RefreshToken = tokenResponse.RefreshToken,
                SessionExpiresAt = sessionExpiresAt
            };

            _dbContext.BffSessions.Add(session);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                SessionCookieName,
                session.Id.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    MaxAge = sessionExpiresAt - utcNow
                });
        }

        public async Task<Tokens> GetTokensAsync(CancellationToken cancellationToken = default)
        {
            Guid? sessionId = ReadSessionIdFromCookie();

            if (sessionId == null)
            {
                return null;
            }

            BffSession session = await _dbContext.BffSessions
                .FirstOrDefaultAsync(bool (BffSession x) => x.Id == sessionId.Value, cancellationToken);

            if (session == null)
            {
                DeleteSessionCookie();
                return null;
            }

            if (session.SessionExpiresAt <= _dateTimeService.UtcNow)
            {
                _dbContext.BffSessions.Remove(session);
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

            BffSession session = await _dbContext.BffSessions.FirstOrDefaultAsync(
                bool (BffSession bffSession) => bffSession.Id == sessionId.Value,
                cancellationToken);

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

            BffSession session = await _dbContext.BffSessions.FirstOrDefaultAsync(
                bool (BffSession x) => x.Id == sessionId.Value,
                cancellationToken);

            if (session != null)
            {
                _dbContext.BffSessions.Remove(session);
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
