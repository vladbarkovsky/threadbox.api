using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Threads.Commands
{
    public class CreateThread : IRequestHandler<CreateThread.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public string Title { get; set; }
            public string Text { get; set; }
            public Guid BoardId { get; set; }

            // NOTE: This property will be null in case we pass empty array from client,
            // because we use multipart/form-data encoding for this request.
            public List<IFormFile> ThreadImages { get; set; } = new();

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                    RuleFor(x => x.Text).NotEmpty().MaximumLength(131072);
                    RuleFor(x => x.BoardId).NotEmpty();

                    RuleFor(x => x.ThreadImages)
                        // TODO: Collection length validator.
                        .Must(x => x.Count <= 5)
                        .WithMessage("Maximum allowed number of files is 5.")

                        .ForEach(x => x.ValidateImage())
                        .WithUnique(x => x.FileName);
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
            var thread = new ORM.Entities.Thread()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Text = request.Text,
                BoardId = request.BoardId,
            };

            _dbContext.Add(thread);

            foreach (var threadImage in request.ThreadImages)
            {
                await SaveThreadImage(threadImage, thread.Id);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task SaveThreadImage(IFormFile formFile, Guid threadId)
        {
            var filePath = $"Images/ThreadImages/{threadId}/{formFile.FileName}";

            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            var data = memoryStream.ToArray();

            await _fileStorage.SaveFileAsync(filePath, data);

            var threadImage = new ThreadImage
            {
                ThreadId = threadId,
                FileInfo = new ORM.Entities.FileInfo
                {
                    Name = formFile.FileName,
                    ContentType = formFile.ContentType,
                    Path = filePath
                }
            };

            _dbContext.ThreadImages.Add(threadImage);
        }
    }
}