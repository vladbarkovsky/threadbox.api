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

namespace ThreadboxApi.Application.Threads.Commands
{
    public sealed class DeleteThread : IRequestHandler<DeleteThread.Command, NoContentResult>
    {
        public sealed class Command : IRequest<NoContentResult>, IConsistentCommand
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

        public DeleteThread(ApplicationDbContext dbContext, IFileStorage fileStorage)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
        }

        public async Task<NoContentResult> Handle(Command request, CancellationToken cancellationToken)
        {
            ORM.Entities.Thread thread = await _dbContext.Threads
                .Where(x => x.Id == request.Id)
                .Include(x => x.ThreadImages)
                .ThenInclude(x => x.FileInfo)
                .SingleOrDefaultAsync(cancellationToken);

            if (thread == null)
            {
                throw new HttpResponseException($"File info with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            thread.Deleted = true;
            _dbContext.SetOriginalRowVersion(thread, request.RowVersion);

            foreach (ThreadImage image in thread.ThreadImages)
            {
                image.Deleted = true;
                image.FileInfo.Deleted = true;
            }
            
            await _fileStorage.DeleteFilesAsync(
                cancellationToken,
                thread.ThreadImages.Select(string (ThreadImage image) => image.FileInfo.Path).ToArray());

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new NoContentResult();
        }
    }
}
