using FluentValidation;
using MediatR;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Sections.Commands
{
    public class CreateSection : IRequestHandler<CreateSection.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public string Title { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;

        public CreateSection(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var section = new Section
            {
                Title = request.Title
            };

            _dbContext.Sections.Add(section);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}