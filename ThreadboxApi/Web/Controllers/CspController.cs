using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Csp.Commands;

namespace ThreadboxApi.Web.Controllers
{
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