using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using System.Security.Cryptography;
using System.Text;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Identity;
using ThreadboxApi.Web.Constants;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class ContinueLogin : IRequestHandler<ContinueLogin.Command, RedirectResult>
    {
        public const string OfflineAccessCookieName = "bff_offline_access";

        public class Command : IRequest<RedirectResult> { }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public ContinueLogin(IHttpContextAccessor httpContextAccessor, IOptionsSnapshot<AppSettings> appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings;
        }

        public async Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            AuthenticateResult authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(Microsoft.AspNetCore.Identity.IdentityConstants.ApplicationScheme);

            if (!authenticateResult.Succeeded)
            {
                string loginUrl = QueryHelpers.AddQueryString(
                    uri: UrlConstants.SignInPath,
                    name: "returnUrl",
                    value: UrlConstants.ContinueLoginPath);

                return new RedirectResult(loginUrl);
            }

            bool offlineAccess = GetOfflineAccess(_httpContextAccessor.HttpContext, authenticateResult);
            string bffState = Guid.NewGuid().ToString();
            string bffNonce = Guid.NewGuid().ToString();
            string codeVerifier = CryptoRandom.CreateUniqueId();
            string codeChallenge = CreateCodeChallenge(codeVerifier);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(5)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                key: "bff_state",
                value: bffState,
                options: cookieOptions);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                key: "bff_nonce",
                value: bffNonce,
                options: cookieOptions);

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                key: "bff_code_verifier",
                value: codeVerifier,
                options: cookieOptions);

            var scopes = new List<string>
            {
                "openid",
                "profile"
            };

            if (offlineAccess)
            {
                scopes.Add("offline_access");
            }

            scopes.Add(ApplicationOidcConstants.ApiScope);

            var query = new Dictionary<string, string>
            {
                { "client_id", ApplicationOidcConstants.ClientId },
                { "redirect_uri", _appSettings.Value.BaseUrl + UrlConstants.PostLoginRedirectCallbackPath },
                { "response_type", "code" },
                { "scope", string.Join(" ", scopes) },
                { "state", bffState },
                { "nonce", bffNonce },
                { "code_challenge", codeChallenge },
                { "code_challenge_method", OpenIddictConstants.CodeChallengeMethods.Sha256 }
            };

            string redirectUrl = QueryHelpers.AddQueryString(_appSettings.Value.BaseUrl + UrlConstants.ConnectAuthorizePath, query);
            return new RedirectResult(redirectUrl);
        }

        private static bool GetOfflineAccess(HttpContext httpContext, AuthenticateResult authenticateResult)
        {
            string cookieValue = httpContext.Request.Cookies[OfflineAccessCookieName];

            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                httpContext.Response.Cookies.Delete(
                    key: OfflineAccessCookieName,
                    options: new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                return bool.TryParse(cookieValue, out bool parsedCookieValue) && parsedCookieValue;
            }

            return authenticateResult.Properties?.IsPersistent == true;
        }

        private static string CreateCodeChallenge(string codeVerifier)
        {
            byte[] hash = SHA256.HashData(Encoding.ASCII.GetBytes(codeVerifier));
            return IdentityModel.Base64Url.Encode(hash);
        }
    }
}
