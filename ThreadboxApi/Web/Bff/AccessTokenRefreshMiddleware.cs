using IdentityModel.Client;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;

namespace ThreadboxApi.Web.Bff
{
    public class AccessTokenRefreshMiddleware : IMiddleware, ITransientService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BffTokensService _bffService;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly IDateTimeService _dateTimeService;

        public AccessTokenRefreshMiddleware(
            IHttpClientFactory httpClientFactory,
            BffTokensService bffService,
            IOptionsSnapshot<AppSettings> appSettings,
            IDateTimeService dateTimeService)
        {
            _httpClientFactory = httpClientFactory;
            _bffService = bffService;
            _appSettings = appSettings;
            _dateTimeService = dateTimeService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tokens = await _bffService.GetTokensAsync();

            if (tokens == null || _dateTimeService.UtcNow.AddMinutes(2) < tokens.ExpiresAt)
            {
                await next(context);
                return;
            }

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                TokenResponse tokenResponse = await httpClient.RequestRefreshTokenAsync(
                    new RefreshTokenRequest
                    {
                        Address = _appSettings.Value.BaseUrl + "/connect/token",
                        ClientId = "bff",
                        ClientSecret = _appSettings.Value.OidcBffClientSecret,
                        RefreshToken = tokens.RefreshToken
                    },
                    context.RequestAborted);

                if (tokenResponse.IsError)
                {
                    await _bffService.ClearTokensAsync(context.RequestAborted);
                }
                else
                {
                    await _bffService.UpdateTokensAsync(tokenResponse, context.RequestAborted);
                }
            }

            await next(context);
        }
    }
}
