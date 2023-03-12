﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Dtos;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

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
			var boards = await _dbContext.Boards.AsNoTracking().ToListAsync();

			var listBoardDtos = _mapper.Map<List<ListBoardDto>>(boards);
			return listBoardDtos;
		}

		public async Task<BoardDto> GetBoardAsync(Guid boardId)
		{
			var board = await _dbContext.Boards
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == boardId);

			HttpResponseExceptions.ThrowNotFoundIfNull(board);
			var boardDto = _mapper.Map<BoardDto>(board);
			return boardDto;
		}

		public async Task<ListBoardDto> CreateBoardAsync(BoardDto boardDto)
		{
			var board = _mapper.Map<Board>(boardDto);
			var addedBoard = _dbContext.Add(board);
			var listBoardDto = _mapper.Map<ListBoardDto>(addedBoard.Entity);
			await _dbContext.SaveChangesAsync();
			return listBoardDto;
		}

		public async Task<ListBoardDto> EditBoardAsync(BoardDto boardDto)
		{
			var board = _mapper.Map<Board>(boardDto);
			var editedBoard = _dbContext.Boards.Update(board);
			var listBoardDto = _mapper.Map<ListBoardDto>(editedBoard.Entity);
			await _dbContext.SaveChangesAsync();
			return listBoardDto;
		}

		public async Task DeleteBoardAsync(Guid boardId)
		{
			var board = await _dbContext.Boards.FindAsync(boardId);
			HttpResponseExceptions.ThrowNotFoundIfNull(board);
			_dbContext.Boards.Remove(board);
			await _dbContext.SaveChangesAsync();
		}
	}
}