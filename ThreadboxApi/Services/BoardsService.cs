using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Dtos;

namespace ThreadboxApi.Services
{
	public class BoardsService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly IMapper _mapper;

		public BoardsService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_mapper = services.GetRequiredService<IMapper>();
		}

		public async Task<List<ListBoardDto>> GetBoardsListAsync()
		{
			var boards = await _dbContext.Boards.ToListAsync();
			return _mapper.Map<List<ListBoardDto>>(boards);
		}

		public async Task<BoardDto> TryGetBoardAsync(Guid boardId)
		{
			var board = await _dbContext.Boards.FindAsync(boardId);
			return _mapper.Map<BoardDto>(board);
		}
	}
}