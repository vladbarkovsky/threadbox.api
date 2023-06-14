using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThreadsController : ControllerBase
    {
        private readonly ThreadsService _threadsService;

        public ThreadsController(IServiceProvider services)
        {
            _threadsService = services.GetRequiredService<ThreadsService>();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<PaginatedResult<ListThreadDto>>> GetThreadsByBoard(Guid boardId, PaginationParamsDto paginationParamsDto)
        {
            return await _threadsService.GetThreadsByBoardAsync(boardId, paginationParamsDto);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ListThreadDto>> CreateThread([FromForm] ThreadDto threadDto)
        {
            return await _threadsService.CreateThreadAsync(threadDto);
        }
    }
}