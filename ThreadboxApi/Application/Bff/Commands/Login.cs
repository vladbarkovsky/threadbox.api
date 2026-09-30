using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class Login : IRequestHandler<Login.Command, RedirectResult>
    {
        public class Command : IRequest<RedirectResult> { }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public Login(IHttpContextAccessor httpContextAccessor, IOptionsSnapshot<AppSettings> appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings;
        }

        public Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            string state = Guid.NewGuid().ToString();

            _httpContextAccessor.HttpContext.Response.Cookies.Append("bff_state", state, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(5)
            });

            string[] scopes = new string[]
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                "threadbox_api.access"
            };

            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "client_id", "bff" },
                { "redirect_uri", _appSettings.Value.BaseUrl + "/api/bff/post-login-redirect-callback" },
                { "response_type", "code" },
                { "scope", string.Join(" ", scopes) },
                { "state", state }
            };

            string redirectUrl = QueryHelpers.AddQueryString(_appSettings.Value.BaseUrl + "/connect/authorize", query);
            return Task.FromResult(new RedirectResult(redirectUrl));
        }
    }
}