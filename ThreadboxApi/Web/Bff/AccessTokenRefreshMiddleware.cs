using IdentityModel.Client;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Bff;
using ThreadboxApi.Application.Bff.Models;
using ThreadboxApi.Configuration;
using ThreadboxApi.ServiceLifetimes;
using ThreadboxApi.Web.Constants;
using ThreadboxApi.Time;

namespace ThreadboxApi.Web.Bff
{
    public class AccessTokenRefreshMiddleware : IMiddleware, IScoped
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly BffService _bffService;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<AccessTokenRefreshMiddleware> _logger;

        public AccessTokenRefreshMiddleware(
            IHttpClientFactory httpClientFactory,
            BffService bffService,
            IOptionsSnapshot<AppSettings> appSettings,
            IDateTimeService dateTimeService,
            ILogger<AccessTokenRefreshMiddleware> logger)
        {
            _httpClientFactory = httpClientFactory;
            _bffService = bffService;
            _appSettings = appSettings;
            _dateTimeService = dateTimeService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Tokens tokens = await _bffService.GetTokensAsync();

            if (tokens == null || string.IsNullOrWhiteSpace(tokens.RefreshToken))
            {
                await next(context);
                return;
            }

            if (tokens.ExpiresAt > _dateTimeService.UtcNow)
            {
                await next(context);
                return;
            }

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                TokenResponse tokenResponse = await httpClient.RequestRefreshTokenAsync(
                    new RefreshTokenRequest
                    {
                        Address = _appSettings.Value.BaseUrl + UrlConstants.ConnectTokenPath,
                        ClientId = "bff",
                        ClientSecret = _appSettings.Value.OidcBffClientSecret,
                        RefreshToken = tokens.RefreshToken
                    },
                    cancellationToken: context.RequestAborted);

                if (tokenResponse.IsError)
                {
                    _logger.LogError(
                        tokenResponse.Exception,
                        "Refresh token request failed. Error: {Error}. Description: {ErrorDescription}. Status code: {StatusCode}.",
                        tokenResponse.Error,
                        tokenResponse.ErrorDescription,
                        (int)tokenResponse.HttpStatusCode);

                    await _bffService.ClearTokensAsync(cancellationToken: context.RequestAborted);
                }
                else
                {
                    await _bffService.UpdateTokensAsync(tokenResponse, cancellationToken: context.RequestAborted);
                }
            }

            await next(context);
        }
    }
}
