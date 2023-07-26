using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common.Helpers.Validation;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Threads.Commands
{
    public class CreateThread : IRequestHandler<CreateThread.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public string Title { get; set; }
            public string Text { get; set; }
            public Guid BoardId { get; set; }
            public List<IFormFile> ThreadImages { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                    RuleFor(x => x.Text).NotEmpty().MaximumLength(131072);
                    RuleFor(x => x.BoardId).NotEmpty();
                    RuleFor(x => x.ThreadImages).ForEach(x => x.ValidateImage());
                }
            }
        }

        private readonly AppDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public CreateThread(AppDbContext dbContext, IFileStorage fileStorage)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var thread = new Domain.Entities.Thread()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Text = request.Text,
                BoardId = request.BoardId,
            };

            if (request.ThreadImages == null || !request.ThreadImages.Any())
            {
                return Unit.Value;
            }
        }
    }
}