using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Pagination;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Threads.Queries
{
    public class SearchThreads : IRequestHandler<SearchThreads.Query, PaginatedResult<ThreadSearchDto>>
    {
        public class Query : PaginatedQuery, IRequest<PaginatedResult<ThreadSearchDto>>
        {
            public Guid BoardId { get; set; }
            public string SearchText { get; set; }

            public class Validator : PaginatedQueryValidatorTemplate<Query>
            {
                protected override void OnConfiguring()
                {
                    RuleFor(x => x.BoardId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public SearchThreads(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ThreadSearchDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var threads = await _dbContext.Threads
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.BoardId == request.BoardId)
                .Include(t => t.Posts
                    .Where(p => p.Text.ToLower().Contains(request.SearchText.ToLower()))
                    .OrderByDescending(x => x.CreatedAt))
                .ThenInclude(x => x.PostImages)
                .Where(t =>
                    t.Title.ToLower().Contains(request.SearchText.ToLower()) ||
                    t.Text.ToLower().Contains(request.SearchText.ToLower()) ||
                    t.Posts.Any(p => p.Text.ToLower().Contains(request.SearchText.ToLower())))
                .Include(x => x.ThreadImages)
                .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt)
                .ToPaginatedResultAsync(request, cancellationToken);

            var paginatedResult = _mapper.Map<PaginatedResult<ThreadSearchDto>>(threads);
            return paginatedResult;
        }
    }
}