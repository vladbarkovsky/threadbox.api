using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Boards.Commands
{
    public class UpdateBoard : IRequestHandler<UpdateBoard.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Id).NotEmpty();
                    RuleFor(x => x.Title).NotEmpty();
                }
            }
        }

        private readonly ThreadboxDbContext _dbContext;

        public UpdateBoard(ThreadboxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var board = await _dbContext.Boards.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

            if (board == null)
            {
                throw HttpResponseException.NotFound;
            }

            board.Title = request.Title;
            board.Description = request.Description;

            _dbContext.Boards.Update(board);
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}