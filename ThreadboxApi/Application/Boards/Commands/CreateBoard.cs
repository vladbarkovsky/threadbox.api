using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Controllers;

namespace ThreadboxApi.Application.Boards.Commands
{
    public class CreateBoard : IRequestHandler<CreateBoard.Command, CreatedAtActionResult>
    {
        public class Command : IRequest<CreatedAtActionResult>
        {
            public Guid SectionId { get; init; }
            public string Title { get; init; }
            public string Description { get; init; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                    RuleFor(x => x.Description).NotEmpty().MaximumLength(2048);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateBoard(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CreatedAtActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var board = new Board
            {
                SectionId = request.SectionId,
                Title = request.Title,
                Description = request.Description
            };

            _dbContext.Add(board);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedAtActionResult(
                actionName: nameof(BoardController.Get),
                controllerName: nameof(BoardController),
                routeValues: new { id = board.Id },
                value: _mapper.Map<BoardDto>(board));
        }
    }
}
