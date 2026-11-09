using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Posts.Commands
{
    public class DeletePost : IRequestHandler<DeletePost.Command, NoContentResult>
    {
        public class Command : IRequest<NoContentResult>, IConsistentCommand
        {
            public Guid Id { get; init; }
            public uint? RowVersion { get; init; }

            public class Validator : ConsistentCommandValidatorTemplate<Command>
            {
                protected override void Configure()
                {
                    RuleFor(x => x.Id).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public DeletePost(ApplicationDbContext dbContext, IFileStorage fileStorage)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
        }

        public async Task<NoContentResult> Handle(Command request, CancellationToken cancellationToken)
        {
            Post post = await _dbContext.Posts
                .Where(x => x.Id == request.Id)
                .Include(x => x.PostImages)
                .ThenInclude(x => x.FileInfo)
                .SingleOrDefaultAsync(cancellationToken);

            if (post == null)
            {
                throw new HttpResponseException($"Post with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            post.Deleted = true;
            _dbContext.SetOriginalRowVersion(post, request.RowVersion);

            foreach (PostImage image in post.PostImages)
            {
                image.Deleted = true;
                image.FileInfo.Deleted = true;
            }

            await _fileStorage.DeleteFilesAsync(
                cancellationToken,
                post.PostImages.Select(string (PostImage image) => image.FileInfo.Path).ToArray());

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new NoContentResult();
        }
    }
}
