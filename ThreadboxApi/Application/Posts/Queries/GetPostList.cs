using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Posts.Queries
{
    public class GetPostList : IRequestHandler<GetPostList.Query, PostDto[]>
    {
        public class Query : IRequest<PostDto[]>
        {
            public Guid ThreadId { get; init; }
            public bool All { get; init; }

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

        public GetPostList(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PostDto[]> Handle(Query request, CancellationToken cancellationToken)
        {
            IQueryable<Post> query = _dbContext.Posts
                .AsNoTracking()
                .Where(x => x.ThreadId == request.ThreadId)
                .Include(x => x.Tripcode)
                .Include(x => x.PostImages)
                .OrderByDescending(x => x.CreatedAt)
                .AsQueryable();

            if (!request.All)
            {
                query = query.Take(3);
            }

            Post[] posts = await query.ToArrayAsync(cancellationToken);
            return _mapper.Map<PostDto[]>(posts);
        }
    }
}