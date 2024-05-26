using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Exceptions;

namespace ThreadboxApi.Application.Posts.Commands
{
    public class DeletePost : IRequestHandler<DeletePost.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public Guid PostId { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.PostId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;

        public DeletePost(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await _dbContext.Posts
                .Where(x => x.Id == request.PostId)
                .Include(x => x.PostImages)
                .ThenInclude(x => x.FileInfo)
                .SingleOrDefaultAsync(cancellationToken);

            HttpStatusException.ThrowNotFoundIfNull(post);

            post.Deleted = true;

            foreach (var postImage in post.PostImages)
            {
                postImage.Deleted = true;
                postImage.FileInfo.Deleted = true;
            }

            _dbContext.Update(post);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
