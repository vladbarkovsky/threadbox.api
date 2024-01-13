using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Posts.Queries
{
    public class GetPostsByThread : IRequestHandler<GetPostsByThread.Query, List<PostDto>>
    {
        public class Query : IRequest<List<PostDto>>
        {
            public Guid ThreadId { get; set; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.ThreadId).NotEmpty();
                }
            }
        }

        private readonly Infrastructure.Persistence.ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetPostsByThread(Infrastructure.Persistence.ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var posts = await _dbContext.Posts
                .Where(x => x.ThreadId == request.ThreadId)
                .Include(x => x.PostImages)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<PostDto>>(posts);
            return dtos;
        }
    }
}