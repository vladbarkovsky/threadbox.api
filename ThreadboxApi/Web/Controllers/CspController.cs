using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Csp.Commands;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/csp")]
    public class CspController : MediatRController
    {
        [HttpPost("[action]")]
        public async Task<ActionResult> CreateCspReport(CreateCspReport.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}