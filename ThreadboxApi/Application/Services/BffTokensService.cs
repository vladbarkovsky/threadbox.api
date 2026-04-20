using IdentityModel.Client;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ThreadboxApi.Application.Bff.Models;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Application.Services
{
    public class BffTokensService : IScopedService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BffTokensService(
            IDistributedCache distributedCache,
            IOptionsMonitor<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _distributedCache = distributedCache;
            _appSettings = appSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SetTokensAsync(TokenResponse tokenResponse, CancellationToken cancellationToken = default)
        {
            ClearTokens();
            var sessionId = Guid.NewGuid().ToString();

            var sessionLifetime = TimeSpan.FromSeconds(tokenResponse.RefreshToken == null ?
                tokenResponse.ExpiresIn :
                _appSettings.CurrentValue.AbsoluteRefreshTokenLifetimeSeconds);

            await _distributedCache.SetStringAsync(
                sessionId,
                JsonSerializer.Serialize(new Tokens
                {
                    AccessToken = tokenResponse.AccessToken,
                    ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                    RefreshToken = tokenResponse.RefreshToken
                }),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(tokenResponse.RefreshToken == null ?
                        tokenResponse.ExpiresIn :
                        _appSettings.CurrentValue.AbsoluteRefreshTokenLifetimeSeconds)

                },
                cancellationToken);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                "bff_session_id",
                tokenResponse.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    MaxAge = sessionLifetime
                });
        }

        public async Task<Tokens> GetTokensAsync(CancellationToken cancellationToken = default)
        {
            var sessionId = _httpContextAccessor.HttpContext.Request.Cookies["bff_session_id"];

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return null;
            }

            var tokensJson = await _distributedCache.GetStringAsync(sessionId, cancellationToken);

            if (string.IsNullOrWhiteSpace(tokensJson))
            {
                return null;
            }

            return JsonSerializer.Deserialize<Tokens>(tokensJson);
        }

        public void ClearTokens()
        {
            var sessionId = _httpContextAccessor.HttpContext.Request.Cookies["bff_session_id"];

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return;
            }

            _distributedCache.Remove(sessionId);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_session_id");
        }
    }
}
