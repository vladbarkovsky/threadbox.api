using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Pagination;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Threads.Queries
{
    public class GetThreadsByBoard : IRequestHandler<GetThreadsByBoard.Query, PaginatedResult<ThreadDto>>
    {
        public class Query : PaginatedQuery, IRequest<PaginatedResult<ThreadDto>>
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

        public GetThreadsByBoard(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ThreadDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            // NOTE: We have to use ordering by ID for each entity to make query return the correct data.
            // See Warning section in https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries#split-queries

            var threadsQuery = _dbContext.Threads
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.BoardId == request.BoardId);

            var hasSearchText = !string.IsNullOrEmpty(request.SearchText);

            if (hasSearchText)
            {
                // TODO: Use some framework for global search.

                threadsQuery = threadsQuery
                    // Include posts containing search text.
                    .Include(t => t.Posts
                        .Where(p => p.Text.ToLower().Contains(request.SearchText.ToLower()))
                        .OrderBy(x => x.Id)
                        .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt))
                    .ThenInclude(x => x.PostImages
                        .OrderBy(x => x.Id))
                    // Select threads containig search text and threads containing posts that contain search text.
                    .Where(t =>
                        t.Title.ToLower().Contains(request.SearchText.ToLower()) ||
                        t.Text.ToLower().Contains(request.SearchText.ToLower()) ||
                        t.Posts.Any(p => p.Text.ToLower().Contains(request.SearchText.ToLower())));
            }
            else
            {
                threadsQuery = threadsQuery
                    .Include(t => t.Posts
                        .OrderBy(x => x.Id)
                        .OrderByDescending(x => x.CreatedAt)
                        /// We need 4 posts if there are no search text specified.
                        /// Based on 4th post presence we will determine <see cref="ThreadDto.HasMorePosts"/> value.
                        .Take(4))
                    .ThenInclude(x => x.PostImages
                        .OrderBy(x => x.Id));
            }

            var threads = await threadsQuery
                .Include(x => x.ThreadImages
                    .OrderBy(x => x.Id))
                .OrderBy(x => x.Id)
                .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt)
                .ToPaginatedResultAsync(request, cancellationToken);

            var paginatedResult = _mapper.Map<PaginatedResult<ThreadDto>>(threads, o =>
            {
                if (!hasSearchText)
                {
                    o.AfterMap((s, d) =>
                    {
                        foreach (var thread in d.PageItems)
                        {
                            thread.HasMorePosts = thread.Posts.Count > 3;
                            // List of post entities contains maximum of 4 posts if no search text specified.
                            // We need a maximum of 3.
                            thread.Posts = thread.Posts.Take(3).ToList();
                        }
                    });
                }
            });

            return paginatedResult;
        }
    }
}