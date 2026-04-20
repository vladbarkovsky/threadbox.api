using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Bff.Commands;

namespace ThreadboxApi.Web.Controllers
{
    public class BffController : MediatRController
    {
        [HttpGet("login")]
        public async Task<ActionResult> Login([FromBody] Login.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("post-login-redirect-callback")]
        public async Task<ActionResult> PostLoginRedirectCallback([FromQuery] PostLoginRedirectCallback.Command query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            return await Mediator.Send(new Logout.Command());
        }

        [HttpGet("post-logout-redirect-callback")]
        public async Task<ActionResult> PostLogoutRedirectCallback()
        {
            return await Mediator.Send(new PostLogoutRedirectCallback.Command());
        }
    }
}
