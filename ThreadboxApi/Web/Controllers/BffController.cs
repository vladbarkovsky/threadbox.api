using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Bff.Commands;

namespace ThreadboxApi.Web.Controllers
{
    [Route("bff")]
    public sealed class BffController : ApplicationController
    {
        public BffController(ISender sender) : base(sender) { }

        [HttpGet("login")]
        public async Task<RedirectResult> Login()
        {
            return await _sender.Send(new Login.Command());
        }

        [HttpGet("continue-login")]
        public async Task<RedirectResult> ContinueLogin()
        {
            return await _sender.Send(new ContinueLogin.Command());
        }

        [HttpGet("post-login-redirect-callback")]
        public async Task<RedirectResult> PostLoginRedirectCallback([FromQuery] PostLoginRedirectCallback.Command query)
        {
            return await _sender.Send(query);
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<RedirectResult> Logout()
        {
            return await _sender.Send(new Logout.Command());
        }

        [HttpGet("post-logout-redirect-callback")]
        public async Task<RedirectResult> PostLogoutRedirectCallback()
        {
            return await _sender.Send(new PostLogoutRedirectCallback.Command());
        }
    }
}
