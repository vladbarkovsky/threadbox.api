using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Dtos;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Tools;

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
				.ToListAsync();

			return _mapper.Map<List<ListThreadDto>>(threads).ToPaginatedListDto(paginationParamsDto);
		}
	}
}