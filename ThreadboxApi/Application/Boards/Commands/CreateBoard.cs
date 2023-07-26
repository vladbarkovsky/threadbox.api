using FluentValidation;
using MediatR;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Boards.Commands
{
    public class CreateBoard : IRequestHandler<CreateBoard.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public string Title { get; set; }
            public string Description { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty();
                }
            }
        }

        private readonly AppDbContext _dbContext;

        public CreateBoard(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var board = new Board
            {
                Title = request.Title,
                Description = request.Description
            };

            _dbContext.Add(board);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}