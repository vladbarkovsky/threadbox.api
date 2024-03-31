using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.ORM.Entities.Interfaces;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Sections.Commands
{
    public class UpdateSection : IRequestHandler<UpdateSection.Command, Unit>
    {
        public class Command : IRequest<Unit>, IConsistent
        {
            public Guid Id { get; set; }
            public byte[] RowVersion { get; set; }
            public string Title { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Id).NotEmpty();
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;

        public UpdateSection(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var section = await _dbContext.Sections
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            section.RowVersion = request.RowVersion;
            section.Title = request.Title;

            _dbContext.Update(section);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}