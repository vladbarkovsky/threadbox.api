using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Threads.Commands
{
    public class DeleteThread : IRequestHandler<DeleteThread.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public Guid ThreadId { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.ThreadId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;

        public DeleteThread(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var thread = await _dbContext.Threads
                .Where(x => x.Id == request.ThreadId)
                .SingleOrDefaultAsync(cancellationToken);

            HttpResponseException.ThrowNotFoundIfNull(thread);

            thread.Deleted = true;
            _dbContext.Threads.Update(thread);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}