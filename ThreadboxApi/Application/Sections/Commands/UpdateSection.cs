using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Sections.Commands
{
    public class UpdateSection : IRequestHandler<UpdateSection.Command, SectionDto>
    {
        public class Command : IRequest<SectionDto>, IConsistentCommand
        {
            [FromRoute]
            public Guid Id { get; init; }

            [FromBody]
            public uint? RowVersion { get; init; }

            [FromBody]
            public string Title { get; init; }

            public class Validator : ConsistentCommandValidatorTemplate<Command>
            {
                protected override void Configure()
                {
                    RuleFor(x => x.Id).NotEmpty();
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateSection(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SectionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            Section section = await _dbContext.Sections
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (section == null)
            {
                throw new HttpResponseException($"Section with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            section.Title = request.Title;

            _dbContext.SetOriginalRowVersion(section, request.RowVersion);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<SectionDto>(section);
        }
    }
}