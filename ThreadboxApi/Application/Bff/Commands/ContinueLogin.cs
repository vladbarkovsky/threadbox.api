using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;

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
            AuthenticateResult authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            if (!authenticateResult.Succeeded)
            {
                string loginUrl = QueryHelpers.AddQueryString(
                    uri: "/Identity/Account/Login",
                    name: "returnUrl",
                    value: "/bff/continue-login");

                return new RedirectResult(loginUrl);
            }

            bool offlineAccess = GetOfflineAccess(_httpContextAccessor.HttpContext, authenticateResult);
            string bffState = Guid.NewGuid().ToString();

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                key: "bff_state",
                value: bffState,
                options: new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(5)
                });

            var scopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile
            };

            if (offlineAccess)
            {
                scopes.Add(IdentityServerConstants.StandardScopes.OfflineAccess);
            }

            scopes.Add("threadbox_api.access");

            var query = new Dictionary<string, string>
            {
                { "client_id", "bff" },
                { "redirect_uri", _appSettings.Value.BaseUrl + "/bff/post-login-redirect-callback" },
                { "response_type", "code" },
                { "scope", string.Join(" ", scopes) },
                { "state", bffState }
            };

            string redirectUrl = QueryHelpers.AddQueryString(_appSettings.Value.BaseUrl + "/connect/authorize", query);
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
    }
}
