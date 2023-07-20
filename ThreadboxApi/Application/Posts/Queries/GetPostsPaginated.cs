using AutoMapper;
using MediatR;
using ThreadboxApi.Application.Common.Helpers.Pagination;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Boards.Queries
{
    public class GetPostsPaginated : IRequestHandler<GetPostsPaginated.Query, PaginatedResult<PostDto>>
    {
        public class Query : PaginatedQuery, IRequest<PaginatedResult<PostDto>>
        { }

        private readonly ThreadboxDbContext _dbContext;
        private IMapper _mapper;

        public GetPostsPaginated(ThreadboxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
        }
    }
}