using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Common.Pagination;
using ThreadboxApi.Application.Threads.Commands;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.Application.Threads.Queries;

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
    }
}