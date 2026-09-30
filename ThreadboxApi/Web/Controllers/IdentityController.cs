using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity;
using ThreadboxApi.Application.Identity.Queries;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/identity")]
    public class IdentityController : MediatRController
    {
        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<string>>> GetUserPermissions()
        {
            return await Mediator.Send(new GetUserPermissions.Query());
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<UserSession>> GetUserSession()
        {
            return await Mediator.Send(new GetUserSession.Query());
        }
    }
}
