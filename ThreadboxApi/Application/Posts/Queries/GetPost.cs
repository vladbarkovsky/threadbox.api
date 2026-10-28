using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Posts.Queries
{
    public sealed class GetPost : IRequestHandler<GetPost.Query, PostDto>
    {
        public sealed class Query : IRequest<PostDto>
        {
            public Guid Id { get; init; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetPost(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PostDto> Handle(Query request, CancellationToken cancellationToken)
        {
            Post post = await _dbContext.Posts
                .Where(x => x.Id == request.Id)
                .Include(x => x.PostImages)
                .Include(x => x.Tripcode)
                .FirstOrDefaultAsync(cancellationToken);

            if (post == null)
            {
                throw new HttpResponseException($"Post with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            return _mapper.Map<PostDto>(post);
        }
    }
}
