using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Exceptions;

namespace ThreadboxApi.Application.Boards.Commands
{
    public class DeleteBoard : IRequestHandler<DeleteBoard.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public Guid BoardId { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.BoardId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;

        public DeleteBoard(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
        {
            var board = await _dbContext.Boards
                .Where(x => x.Id == request.BoardId)
                .SingleOrDefaultAsync(cancellationToken);

            HttpStatusException.ThrowNotFoundIfNull(board);

            board.Deleted = true;

            _dbContext.Update(board);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}