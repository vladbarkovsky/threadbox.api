using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Posts.Queries
{
    public class GetPosts : IRequestHandler<GetPosts.Query, List<PostDto>>
    {
        public class Query : IRequest<List<PostDto>>
        {
            public Guid ThreadId { get; set; }
            public bool All { get; set; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.ThreadId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetPosts(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Posts
                .Where(x => x.ThreadId == request.ThreadId)
                .Include(x => x.Tripcode)
                .Include(x => x.PostImages)
                .OrderByDescending(x => x.CreatedAt)
                .AsQueryable();

            if (!request.All)
            {
                query = query.Take(3);
            }

            var posts = await query.ToListAsync(cancellationToken);
            var dtos = _mapper.Map<List<PostDto>>(posts);
            return dtos;
        }
    }
}