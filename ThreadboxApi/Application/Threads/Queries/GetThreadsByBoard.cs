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
            var threads = await _dbContext.Threads
                .AsNoTracking()
                .AsSplitQuery()
                .Where(x => x.BoardId == request.BoardId)
                .OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt)
                .Select(t => new ORM.Entities.Thread
                {
                    Id = t.Id,
                    Title = t.Title,
                    Text = t.Text,
                    BoardId = t.BoardId,
                    Posts = t.Posts
                        .OrderByDescending(x => x.CreatedAt)

                        /// We need 4 posts, because based on 4th post presence we will determine
                        /// <see cref="ThreadDto.HasMorePosts"/> value. See <see cref="ThreadDto.Mapping(Profile)"/>.
                        .Take(4)

                        .Select(p => new ORM.Entities.Post
                        {
                            Id = p.Id,
                            Text = p.Text,
                            ThreadId = p.ThreadId,
                            PostImages = p.PostImages,
                        })
                        .ToList(),
                    ThreadImages = t.ThreadImages
                })
                .ToPaginatedResultAsync(request, cancellationToken);

            var paginatedResult = _mapper.Map<PaginatedResult<ThreadDto>>(threads);
            return paginatedResult;
        }
    }
}