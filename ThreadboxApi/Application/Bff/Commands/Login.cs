using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ThreadboxApi.Web.Constants;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class Login : IRequestHandler<Login.Command, RedirectResult>
    {
        public class Command : IRequest<RedirectResult> { }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public Login(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            AuthenticateResult authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            if (authenticateResult.Succeeded)
            {
                return new RedirectResult(UrlConstants.ContinueLoginPath);
            }

            string loginUrl = QueryHelpers.AddQueryString(
                uri: UrlConstants.SignInPath,
                name: "returnUrl",
                value: UrlConstants.ContinueLoginPath);

            return new RedirectResult(loginUrl);
        }
    }
}
