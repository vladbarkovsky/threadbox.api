using IdentityModel.Client;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Web.Bff
{
    public class AccessTokenRefreshMiddleware : IMiddleware, ITransientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BffTokensService _bffService;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public AccessTokenRefreshMiddleware(
            IHttpClientFactory httpClientFactory,
            BffTokensService bffService,
            IOptionsSnapshot<AppSettings> appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _bffService = bffService;
            _appSettings = appSettings;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tokens = await _bffService.GetTokensAsync();

            if (tokens == null || DateTimeOffset.UtcNow.AddMinutes(2) < tokens.ExpiresAt)
            {
                await next(context);
                return;
            }

            var client = _httpClientFactory.CreateClient();

            var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = _appSettings + "/connect/token",
                ClientId = "bff",
                ClientSecret = _appSettings.Value.OidcBffClientSecret,
                RefreshToken = tokens.RefreshToken
            });

            if (tokenResponse.IsError)
            {
                _bffService.ClearTokens();
            }

            await next(context);
        }
    }
}
