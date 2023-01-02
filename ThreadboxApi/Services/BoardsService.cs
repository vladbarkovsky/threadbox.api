using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Dtos;
using ThreadboxApi.Models;

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

		public async Task<ComponentBoardDto> TryGetBoardAsync(Guid boardId)
		{
			var board = await _dbContext.Boards.FindAsync(boardId);
			return _mapper.Map<ComponentBoardDto>(board);
		}

		public async Task<ListBoardDto> CreateBoardAsync(BoardDto boardDto)
		{
			var board = _mapper.Map<Board>(boardDto);
			var addedBoard = await _dbContext.AddAsync(board);
			var listBoardDto = _mapper.Map<ListBoardDto>(addedBoard.Entity);
			await _dbContext.SaveChangesAsync();
			return listBoardDto;
		}

		public async Task<ListBoardDto> EditBoardAsync(Guid boardId, BoardDto boardDto)
		{
			var board = _mapper.Map<Board>(boardDto);
			board.Id = boardId;
			var editedBoard = _dbContext.Boards.Update(board);
			var listBoardDto = _mapper.Map<ListBoardDto>(editedBoard.Entity);
			await _dbContext.SaveChangesAsync();
			return listBoardDto;
		}

		public async Task DeleteBoardAsync(Guid boardId)
		{
			var board = await _dbContext.Boards.FindAsync(boardId);

			if (board == null)
			{
				throw new ArgumentException("Can't find board to delete.", nameof(boardId));
			}

			_dbContext.Boards.Remove(board);
			await _dbContext.SaveChangesAsync();
		}
	}
}