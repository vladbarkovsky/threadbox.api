using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Common;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Boards.Commands
{
    public class UpdateBoard : IRequestHandler<UpdateBoard.Command, BoardDto>
    {
        public class Command : IRequest<BoardDto>, IConsistentCommand
        {
            [FromRoute]
            public Guid Id { get; init; }

            [FromBody]
            public string Title { get; init; }

            [FromBody]
            public string Description { get; init; }

            [FromBody]
            public uint? RowVersion { get; init; }

            public class Validator : ConsistentCommandValidatorTemplate<Command>
            {
                protected override void Configure()
                {
                    RuleFor(x => x.Id).NotEmpty();
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                    RuleFor(x => x.Description).NotEmpty().MaximumLength(2048);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateBoard(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BoardDto> Handle(Command request, CancellationToken cancellationToken)
        {
            Board board = await _dbContext.Boards
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (board == null)
            {
                throw new HttpResponseException($"Board with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            board.Title = request.Title;
            board.Description = request.Description;

            _dbContext.SetOriginalRowVersion(board, request.RowVersion);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BoardDto>(board);
        }
    }
}