using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Infrastructure.Persistence;

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

        private readonly Infrastructure.Persistence.AppDbContext _dbContext;

        public DeleteBoard(Infrastructure.Persistence.AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<Unit> IRequestHandler<Command, Unit>.Handle(Command request, CancellationToken cancellationToken)
        {
            var board = _dbContext.Boards
                .Where(x => x.Id == request.BoardId)
                .FirstOrDefaultAsync(cancellationToken);

            if (board == null)
            {
                throw HttpResponseException.NotFound;
            }

            _dbContext.Remove(board);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}