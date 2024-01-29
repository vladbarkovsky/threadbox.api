using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.Application.Sections.Queries;

namespace ThreadboxApi.Web.Controllers
{
    public class SectionsController : MediatRController
    {
        [HttpGet("[action]")]
        public async Task<ActionResult<List<SectionDto>>> GetSections()
        {
            return await Mediator.Send(new GetSections.Query());
        }
    }
}