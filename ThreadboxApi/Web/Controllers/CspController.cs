using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Csp.Commands;

namespace ThreadboxApi.Web.Controllers
{
    [Route("csp")]
    public sealed class CspController : ApplicationController
    {
        public CspController(ISender sender) : base(sender) { }

        [HttpPost]
        public async Task<NoContentResult> Create([FromBody] CreateCspReport.Command command)
        {
            return await _sender.Send(command);
        }
    }
}
