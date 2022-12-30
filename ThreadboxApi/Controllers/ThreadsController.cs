using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Dtos;
using ThreadboxApi.Services;

namespace ThreadboxApi.Controllers
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
		public async Task<ActionResult<PaginatedListDto<ListThreadDto>>> GetThreadsByBoardAsync(Guid boardId, PaginationParamsDto paginationParamsDto)
		{
			return await _threadsService.GetThreadsByBoardAsync(boardId, paginationParamsDto);
		}
	}
}