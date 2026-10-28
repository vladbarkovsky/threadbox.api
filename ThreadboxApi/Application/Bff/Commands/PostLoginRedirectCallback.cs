using IdentityModel.Client;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class PostLoginRedirectCallback : IRequestHandler<PostLoginRedirectCallback.Command, RedirectResult>
    {
        public class Command : IRequest<RedirectResult>
        {
            public string Code { get; init; }
            public string State { get; init; }
            public string Error { get; init; }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly BffService _bffService;
        private readonly ILogger<PostLoginRedirectCallback> _logger;

        public PostLoginRedirectCallback(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<AppSettings> appSettings,
            BffService bffService,
            ILogger<PostLoginRedirectCallback> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings;
            _bffService = bffService;
            _logger = logger;
        }

        public async Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Error))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_state");
                _logger.LogError("Authorization callback returned an error: {Error}.", request.Error);
                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + "/authorization-error");
            }

            string expectedState = _httpContextAccessor.HttpContext.Request.Cookies["bff_state"];
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_state");

            if (string.IsNullOrWhiteSpace(expectedState) || request.State != expectedState)
            {
                _logger.LogError(
                    "Authorization callback state validation failed. Expected state present: {ExpectedStatePresent}. Received state present: {ReceivedStatePresent}.",
                    !string.IsNullOrWhiteSpace(expectedState),
                    !string.IsNullOrWhiteSpace(request.State));

                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + "/authorization-error");
            }

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                TokenResponse tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(
                    new AuthorizationCodeTokenRequest
                    {
                        Address = _appSettings.Value.BaseUrl + "/connect/token",
                        ClientId = "bff",
                        ClientSecret = _appSettings.Value.OidcBffClientSecret,
                        Code = request.Code,
                        RedirectUri = _appSettings.Value.BaseUrl + "/bff/post-login-redirect-callback"
                    },
                    cancellationToken);

                if (tokenResponse.IsError)
                {
                    _logger.LogError(
                        tokenResponse.Exception,
                        "Authorization code token request failed. Error: {Error}. Description: {ErrorDescription}. Status code: {StatusCode}.",
                        tokenResponse.Error,
                        tokenResponse.ErrorDescription,
                        (int)tokenResponse.HttpStatusCode);

                    return new RedirectResult(_appSettings.Value.FrontendBaseUrl + "/authorization-error");
                }

                await _bffService.SetTokensAsync(tokenResponse, cancellationToken);
            }

            return new RedirectResult(_appSettings.Value.FrontendBaseUrl);
        }
    }
}
