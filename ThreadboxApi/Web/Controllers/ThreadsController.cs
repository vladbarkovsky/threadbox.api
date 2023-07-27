using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Common.Helpers.Pagination;
using ThreadboxApi.Application.Threads.Commands;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.Application.Threads.Queries;

namespace ThreadboxApi.Web.Controllers
{
    public class ThreadsController : MediatRController
    {
        [HttpPost("[action]")]
        public async Task<ActionResult<PaginatedResult<ThreadDto>>> GetThreadsByBoard([FromBody] GetThreadsByBoard.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreateThread([FromBody] CreateThread.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}