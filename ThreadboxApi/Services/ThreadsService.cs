using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Dtos;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Tools;
using ThreadboxApi.Models;
using System.Linq.Expressions;

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

		public async Task<PaginatedResult<ListThreadDto>> GetThreadsByBoardAsync(Guid boardId, PaginationParamsDto paginationParamsDto)
		{
			Expression<Func<ThreadModel, ListThreadDto>> listThreadDtoSelectExpression = thread => new ListThreadDto
			{
				Id = thread.Id,
				Title = thread.Title,
				Text = thread.Text,
				ThreadImageUrls = thread.ThreadImages
					.Select(threadImage => string.Format(Constants.ThreadImageRequest, threadImage.Id))
					.ToList(),
				Posts = thread.Posts
					.Select(post => new ListPostDto
					{
						Id = post.Id,
						ThreadId = post.ThreadId,
						Text = post.Text,
						PostImageUrls = post.PostImages
							.Select(postImage => string.Format(Constants.PostImageRequest, postImage.Id))
							.ToList()
					})
					.ToList()
			};

			var listThreadDtos = await _dbContext.Threads
				.AsNoTracking()
				.AsSplitQuery()
				.Where(x => x.BoardId == boardId)
				.Select(listThreadDtoSelectExpression)
				.ToPaginatedResultAsync(paginationParamsDto);

			return listThreadDtos;
		}

		public async Task<ListThreadDto> CreateThreadAsync(ThreadDto threadDto)
		{
			var thread = _mapper.Map<ThreadModel>(threadDto);
			var createdThread = _dbContext.Threads.Add(thread).Entity;

			var listThreadDto = new ListThreadDto
			{
				Id = createdThread.Id,
				Title = createdThread.Title,
				Text = createdThread.Text,
				ThreadImageUrls = createdThread.ThreadImages
					.Select(threadImage => string.Format(Constants.ThreadImageRequest, threadImage.Id))
					.ToList()
			};

			await _dbContext.SaveChangesAsync();
			return listThreadDto;
		}
	}
}