using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Common.Pagination;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Threads.Commands;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.Application.Threads.Queries;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Web.Controllers
{
    public class ThreadsController : MediatRController
    {
        [HttpPost("[action]")]
        public async Task<ActionResult<PaginatedResult<ThreadDto>>> GetThreads([FromBody] GetThreads.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateThread([FromForm] CreateThread.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        [Permission(ThreadsPermissions.Delete)]
        public async Task<ActionResult> DeleteThread([FromQuery] DeleteThread.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}