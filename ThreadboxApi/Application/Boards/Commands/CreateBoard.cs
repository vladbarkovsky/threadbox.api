using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Data;
using ThreadboxApi.Domain.Common;
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

        private readonly ThreadboxDbContext _dbContext;
        private GuidCombGenerator _guidCombGenerator;

        public CreateBoard(ThreadboxDbContext dbContext, GuidCombGenerator guidCombGenerator)
        {
            _dbContext = dbContext;
            _guidCombGenerator = guidCombGenerator;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var board = new Board
            {
                Id = _guidCombGenerator.Generate(),
                Title = request.Title,
                Description = request.Description
            };

            _dbContext.Add(board);
            await _dbContext.SaveChangesAsync();
            return Unit.Value;
        }
    }
}