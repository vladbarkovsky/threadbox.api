using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Controllers;

namespace ThreadboxApi.Application.Sections.Commands
{
    public class CreateSection : IRequestHandler<CreateSection.Command, CreatedAtActionResult>
    {
        public class Command : IRequest<CreatedAtActionResult>
        {
            public string Title { get; init; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateSection(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CreatedAtActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var section = new Section
            {
                Title = request.Title
            };

            _dbContext.Sections.Add(section);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedAtActionResult(
                actionName: nameof(SectionController.Get),
                controllerName: nameof(SectionController),
                routeValues: new { id = section.Id },
                value: _mapper.Map<SectionDto>(section));
        }
    }
}