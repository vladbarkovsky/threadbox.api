using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Queries;

namespace ThreadboxApi.Web.Controllers
{
    public class IdentityController : MediatRController
    {
        [HttpGet("[action]")]
        [Authorize]
        public async Task<ActionResult<List<string>>> GetUserPermissions()
        {
            return await Mediator.Send(new GetUserPermissions.Query());
        }
    }
}
