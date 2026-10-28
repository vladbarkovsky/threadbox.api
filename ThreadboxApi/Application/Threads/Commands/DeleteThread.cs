using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.ORM.Entities.Interfaces;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Threads.Commands
{
    public class DeleteThread : IRequestHandler<DeleteThread.Command, NoContentResult>
    {
        public class Command : IRequest<NoContentResult>, IConsistentCommand
        {
            [FromRoute]
            public Guid Id { get; init; }

            [FromBody]
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

        public DeleteThread(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NoContentResult> Handle(Command request, CancellationToken cancellationToken)
        {
            ORM.Entities.Thread thread = await _dbContext.Threads
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (thread == null)
            {
                throw new HttpResponseException($"File info with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            thread.Deleted = true;
            _dbContext.SetOriginalRowVersion(thread, request.RowVersion);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new NoContentResult();
        }
    }
}