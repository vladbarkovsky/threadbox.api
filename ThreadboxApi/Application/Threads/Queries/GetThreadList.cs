using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Pagination;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Threads.Queries
{
    public class GetThreadList : IRequestHandler<GetThreadList.Query, PaginatedResult<ThreadDto>>
    {
        public class Query : PaginatedQuery, IRequest<PaginatedResult<ThreadDto>>
        {
            [FromRoute]
            public Guid BoardId { get; init; }

            [FromQuery]
            public  string SearchText { get; init; }

            public class Validator : PaginatedQueryValidatorTemplate<Query>
            {
                protected override void Configure()
                {
                    RuleFor(x => x.BoardId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetThreadList(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ThreadDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            // NOTE: We have to use ordering by ID for each entity to make query return the correct data.
            // See Warning section in https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries#split-queries

            IQueryable<ORM.Entities.Thread> threadsQuery = _dbContext.Threads
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.BoardId == request.BoardId);

            bool hasSearchText = !string.IsNullOrEmpty(request.SearchText);

            if (hasSearchText)
            {
                // TODO: Use some framework for global search.

                threadsQuery = threadsQuery
                    // Include posts containing search text.
                    .Include(x => x.Posts
                        .Where(x => x.Text.ToLower().Contains(request.SearchText.ToLower()))
                        .OrderBy(x => x.Id)
                        .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt))
                    .ThenInclude(x => x.PostImages
                        .OrderBy(x => x.Id))
                    .Include(x => x.Posts
                        .Where(x => x.Text.ToLower().Contains(request.SearchText.ToLower()))
                        .OrderBy(x => x.Id)
                        .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt))
                    .ThenInclude(x => x.Tripcode)
                    // Select threads containig search text and threads containing posts that contain search text.
                    .Where(x =>
                        x.Title.ToLower().Contains(request.SearchText.ToLower()) ||
                        x.Text.ToLower().Contains(request.SearchText.ToLower()) ||
                        x.Posts.Any(post => post.Text.ToLower().Contains(request.SearchText.ToLower())));
            }
            else
            {
                threadsQuery = threadsQuery
                    .Include(x => x.Posts
                        .OrderBy(x => x.Id)
                        .OrderByDescending(x => x.CreatedAt)
                        /// We need 4 posts if there are no search text specified.
                        /// Based on 4th post presence we will determine <see cref="ThreadDto.HasMorePosts"/> value.
                        .Take(4))
                    .ThenInclude(x => x.PostImages
                        .OrderBy(x => x.Id))
                    .Include(x => x.Posts
                        .OrderBy(x => x.Id)
                        .OrderByDescending(x => x.CreatedAt)
                        /// We need 4 posts if there are no search text specified.
                        /// Based on 4th post presence we will determine <see cref="ThreadDto.HasMorePosts"/> value.
                        .Take(4))
                    .ThenInclude(post => post.Tripcode);
            }

            var threads = await threadsQuery
                .Include(x => x.ThreadImages
                    .OrderBy(x => x.Id))
                .Include(x => x.Tripcode)
                .OrderBy(x => x.Id)
                .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt)
                .ToPaginatedResultAsync(request, cancellationToken);

            PaginatedResult<ThreadDto> paginatedResult = _mapper.Map<PaginatedResult<ThreadDto>>(threads, options =>
            {
                if (!hasSearchText)
                {
                    options.AfterMap(void (object source, PaginatedResult<ThreadDto> destination) =>
                    {
                        foreach (ThreadDto thread in destination.PageItems)
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