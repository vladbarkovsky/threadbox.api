using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class Logout : IRequestHandler<Logout.Command, RedirectResult>
    {
        public class Command : IRequest<RedirectResult> { }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public Logout(IHttpContextAccessor httpContextAccessor, IOptionsSnapshot<AppSettings> appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings;
        }

        public async Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var idToken = await _httpContextAccessor.HttpContext.GetTokenAsync("id_token");

            var redirectUrl = new UriBuilder(_appSettings.Value.BaseUrl)
            {
                Path = "/connect/endsession",
            };

            var query = new Dictionary<string, string>
            {
                { "id_token_hint", idToken },
                { "post_logout_redirect_uri", _appSettings.Value.FrontendBaseUrl + "/bff/sign-out-redirect-callback" }
            };

            redirectUrl.Query = QueryHelpers.AddQueryString(redirectUrl.Query, query).TrimStart('?');
            return new RedirectResult(redirectUrl.ToString());
        }
    }
}