using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common.Helpers.Validation;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Domain.Entities;
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

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public CreateThread(ApplicationDbContext dbContext, IFileStorage fileStorage)
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

            foreach (var threadImage in request.ThreadImages)
            {
                await SaveThreadImage(threadImage, thread.Id, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task SaveThreadImage(IFormFile formFile, Guid threadId, CancellationToken cancellationToken)
        {
            var filePath = $"Images/ThreadImages/{threadId}/{formFile.Name}";

            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            var data = memoryStream.ToArray();

            await _fileStorage.SaveFileAsync(filePath, data);

            var threadImage = new ThreadImage
            {
                ThreadId = threadId,
                FileInfo = new Domain.Entities.FileInfo
                {
                    Name = formFile.Name,
                    ContentType = formFile.ContentType,
                    Path = filePath
                }
            };

            _dbContext.ThreadImages.Add(threadImage);
        }
    }
}