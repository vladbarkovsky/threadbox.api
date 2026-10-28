using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

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
                return new RedirectResult("/bff/continue-login");
            }

            string loginUrl = QueryHelpers.AddQueryString(
                uri: "/Identity/Account/Login",
                name: "returnUrl",
                value: "/bff/continue-login");

            return new RedirectResult(loginUrl);
        }
    }
}
