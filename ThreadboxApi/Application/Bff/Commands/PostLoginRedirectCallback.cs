using IdentityModel.Client;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Identity;
using ThreadboxApi.Web.Constants;

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
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_nonce");
                _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_code_verifier");

                _logger.LogError("Authorization callback returned an error: {Error}.", request.Error);
                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + UrlConstants.AuthorizationErrorPath);
            }

            string expectedState = _httpContextAccessor.HttpContext.Request.Cookies["bff_state"];
            string expectedNonce = _httpContextAccessor.HttpContext.Request.Cookies["bff_nonce"];
            string codeVerifier = _httpContextAccessor.HttpContext.Request.Cookies["bff_code_verifier"];

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_state");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_nonce");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("bff_code_verifier");

            if (string.IsNullOrWhiteSpace(expectedState) || request.State != expectedState)
            {
                _logger.LogError(
                    "Authorization callback state validation failed. Expected state present: {ExpectedStatePresent}. Received state present: {ReceivedStatePresent}.",
                    !string.IsNullOrWhiteSpace(expectedState),
                    !string.IsNullOrWhiteSpace(request.State));

                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + UrlConstants.AuthorizationErrorPath);
            }

            if (string.IsNullOrWhiteSpace(codeVerifier))
            {
                _logger.LogError("Authorization callback code verifier is missing.");
                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + UrlConstants.AuthorizationErrorPath);
            }

            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                TokenResponse tokenResponse = await httpClient.RequestAuthorizationCodeTokenAsync(
                    new AuthorizationCodeTokenRequest
                    {
                        Address = _appSettings.Value.BaseUrl + UrlConstants.ConnectTokenPath,
                        ClientId = ApplicationOidcConstants.ClientId,
                        ClientSecret = _appSettings.Value.OidcBffClientSecret,
                        Code = request.Code,
                        CodeVerifier = codeVerifier,
                        RedirectUri = _appSettings.Value.BaseUrl + UrlConstants.PostLoginRedirectCallbackPath
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

                    return new RedirectResult(_appSettings.Value.FrontendBaseUrl + UrlConstants.AuthorizationErrorPath);
                }

                string identityTokenNonce = ReadIdentityTokenNonce(tokenResponse.IdentityToken);

                if (string.IsNullOrWhiteSpace(expectedNonce) || identityTokenNonce != expectedNonce)
                {
                    _logger.LogError(
                        "Identity token nonce validation failed. Expected nonce present: {ExpectedNoncePresent}. Received nonce present: {ReceivedNoncePresent}.",
                        !string.IsNullOrWhiteSpace(expectedNonce),
                        !string.IsNullOrWhiteSpace(identityTokenNonce));

                    return new RedirectResult(_appSettings.Value.FrontendBaseUrl + UrlConstants.AuthorizationErrorPath);
                }

                await _bffService.SetTokensAsync(tokenResponse, cancellationToken);
            }

            return new RedirectResult(_appSettings.Value.FrontendBaseUrl);
        }

        private static string ReadIdentityTokenNonce(string identityToken)
        {
            if (string.IsNullOrWhiteSpace(identityToken))
            {
                return null;
            }

            try
            {
                var jwt = new JsonWebToken(identityToken);

                if (jwt.TryGetPayloadValue(OpenIddictConstants.Claims.Nonce, out string nonce))
                {
                    return nonce;
                }

                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }
    }
}
