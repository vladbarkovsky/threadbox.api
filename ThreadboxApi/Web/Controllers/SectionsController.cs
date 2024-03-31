using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Sections.Commands;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.Application.Sections.Queries;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Web.Controllers
{
    public class SectionsController : MediatRController
    {
        [HttpGet("[action]")]
        public async Task<ActionResult<SectionDto>> GetSection([FromQuery] GetSection.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<SectionListDto>>> GetSections()
        {
            return await Mediator.Send(new GetSections.Query());
        }

        [HttpPost("[action]")]
        [Permission(SectionsPermissions.Manage)]
        public async Task<ActionResult> CreateSection([FromBody] CreateSection.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("[action]")]
        [Permission(SectionsPermissions.Manage)]
        public async Task<ActionResult> UpdateSection([FromBody] UpdateSection.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}