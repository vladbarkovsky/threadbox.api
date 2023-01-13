﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Dtos;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Tools;
using ThreadboxApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ThreadboxApi.Services
{
	public class ThreadsService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly IMapper _mapper;

		public ThreadsService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_mapper = services.GetRequiredService<IMapper>();
		}

		public async Task<PaginatedListDto<ListThreadDto>> GetThreadsByBoardAsync(Guid boardId, PaginationParamsDto paginationParamsDto)
		{
			var threads = await _dbContext.Threads
				.Where(x => x.BoardId == boardId)
				.Include(x => x.ThreadImages)
				.Include(x => x.Posts)
				.ThenInclude(x => x.PostImages)
				.ToListAsync();

			return _mapper.Map<List<ListThreadDto>>(threads).ToPaginatedListDto(paginationParamsDto);
		}

		public async Task<ListThreadDto> CreateThreadAsync(Guid boardId, ThreadDto threadDto)
		{
			var thread = _mapper.Map<ThreadModel>(threadDto);
			thread.BoardId = boardId;

			foreach (var image in thread.ThreadImages)
			{
				image.Thread = thread;
			}

			await _dbContext.ThreadImages.AddRangeAsync(thread.ThreadImages);

			var board = await _dbContext.Boards.FindAsync(boardId);

			if (board == null)
			{
				throw new ArgumentException("Can't find board.", nameof(boardId));
			}

			board.Threads.Add(thread);
			_dbContext.Update(board);

			var createdThread = await _dbContext.Threads.AddAsync(thread);
			var listThreadDto = _mapper.Map<ListThreadDto>(createdThread.Entity);
			await _dbContext.SaveChangesAsync();
			return listThreadDto;
		}
	}
}