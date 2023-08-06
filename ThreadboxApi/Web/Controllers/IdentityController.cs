using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Commands;

namespace ThreadboxApi.Web.Controllers
{
    public class IdentityController : MediatRController
    {
        [HttpPost("[action]")]
        public async Task<ActionResult<string>> SignIn([FromBody] SignIn.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SignUp([FromBody] SignUp.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateRegistrationKey()
        {
            return await Mediator.Send(new CreateRegistrationKey.Command());
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<string>> RefreshAccessToken()
        {
            return await Mediator.Send(new RefreshAccessToken.Command());
        }
    }
}