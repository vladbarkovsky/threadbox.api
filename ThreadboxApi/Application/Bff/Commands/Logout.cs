using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class Logout : IRequestHandler<Logout.Command, RedirectResult>
    {
        public class Command : IRequest<RedirectResult> { }

        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public Logout(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "post_logout_redirect_uri", _appSettings.Value.BaseUrl + "/api/bff/post-logout-redirect-callback" }
            };

            string redirectUrl = QueryHelpers.AddQueryString(_appSettings.Value.BaseUrl + "/connect/endsession", query);
            return Task.FromResult(new RedirectResult(redirectUrl));
        }
    }
}
