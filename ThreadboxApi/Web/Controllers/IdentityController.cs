using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Commands;

namespace ThreadboxApi.Web.Controllers
{
    public class IdentityController : MediatRController
    {
        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateRegistrationKey()
        {
            return await Mediator.Send(new CreateRegistrationKey.Command());
        }
    }
}