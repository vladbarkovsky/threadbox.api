using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Dtos;
using ThreadboxApi.Services;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BoardsController : ControllerBase
	{
		private readonly BoardsService _boardsService;

		public BoardsController(IServiceProvider services)
		{
			_boardsService = services.GetRequiredService<BoardsService>();
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<List<ListBoardDto>>> GetBoardsList()
		{
			return await _boardsService.GetBoardsListAsync();
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<BoardDto>> GetBoard(Guid boardId)
		{
			var boardDto = await _boardsService.TryGetBoardAsync(boardId);
			return boardDto != null ? boardDto : NotFound();
		}

		[HttpPost("[action]")]
		public async Task<ActionResult<ListBoardDto>> CreateBoard(BoardDto boardDto)
		{
			return await _boardsService.CreateBoardAsync(boardDto);
		}

		[HttpPut("[action]")]
		public async Task<ActionResult<ListBoardDto>> EditBoard(BoardDto boardDto)
		{
			return await _boardsService.EditBoardAsync(boardDto);
		}

		[HttpDelete("[action]")]
		public async Task<ActionResult> DeleteBoard(Guid boardId)
		{
			await _boardsService.DeleteBoardAsync(boardId);
			return NoContent();
		}
	}
}