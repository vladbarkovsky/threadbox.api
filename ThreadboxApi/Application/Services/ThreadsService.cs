using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Dtos;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Tools;
using System.Linq.Expressions;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Services
{
    public class ThreadsService : IScopedService
    {
        private readonly Infrastructure.Persistence.DbContext _dbContext;
        private readonly IMapper _mapper;

        public ThreadsService(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<Infrastructure.Persistence.DbContext>();
            _mapper = services.GetRequiredService<IMapper>();
        }

        public async Task<PaginatedResult<ListThreadDto>> GetThreadsByBoardAsync(Guid boardId, PaginationParamsDto paginationParamsDto)
        {
            Expression<Func<Domain.Entities.Thread, ListThreadDto>> listThreadDtoSelectExpression = thread => new ListThreadDto
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
            var thread = _mapper.Map<Domain.Entities.Thread>(threadDto);
            var createdThread = _dbContext.Threads.Add(thread).Entity;
            var listThreadDto = _mapper.Map<ListThreadDto>(createdThread);

            await _dbContext.SaveChangesAsync();
            return listThreadDto;
        }
    }
}