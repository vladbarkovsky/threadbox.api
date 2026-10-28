using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Boards.Commands
{
    public class DeleteBoard : IRequestHandler<DeleteBoard.Command, NoContentResult>
    {
        public class Command : IRequest<NoContentResult>, IConsistentCommand
        {
            [FromRoute]
            public Guid Id { get; init; }
            [FromQuery]
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

        public DeleteBoard(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<NoContentResult> IRequestHandler<Command, NoContentResult>.Handle(Command request, CancellationToken cancellationToken)
        {
            Board board = await _dbContext.Boards
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (board == null)
            {
                throw new HttpResponseException($"Board with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            board.Deleted = true;

            _dbContext.SetOriginalRowVersion(board, request.RowVersion);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new NoContentResult();
        }
    }
}