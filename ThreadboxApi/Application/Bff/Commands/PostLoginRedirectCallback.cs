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
            public string Code { get; set; }
            public string State { get; set; }
            public string Error { get; set; }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly BffTokensService _bffService;

        public PostLoginRedirectCallback(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<AppSettings> appSettings,
            BffTokensService bffService)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings;
            _bffService = bffService;
        }

        public async Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Error))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_state");

                // TODO: Log the error details.
                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + "/authorization-error");
            }

            var expectedState = _httpContextAccessor.HttpContext.Request.Cookies["bff_state"];
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_state");

            if (string.IsNullOrWhiteSpace(expectedState) || request.State != expectedState)
            {
                // TODO: Log the error details.
                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + "/authorization-error");
            }

            var httpClient = _httpClientFactory.CreateClient();

            var tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(
                new AuthorizationCodeTokenRequest
                {
                    Address = _appSettings.Value.BaseUrl + "/connect/token",
                    ClientId = "bff",
                    ClientSecret = _appSettings.Value.OidcBffClientSecret,
                    Code = request.Code,
                    RedirectUri = _appSettings.Value.BaseUrl + "/post-login-redirect-callback"
                },
                cancellationToken);

            if (tokenResponse.IsError)
            {
                // TODO: Log the error details.
                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + "/authorization-error");
            }

            await _bffService.SetTokensAsync(tokenResponse, cancellationToken);
            return new RedirectResult(_appSettings.Value.FrontendBaseUrl);
        }
    }
}
