using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common.Helpers.Validation;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Posts.Commands
{
    public class CreatePost : IRequestHandler<CreatePost.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public string Text { get; set; }
            public List<IFormFile> PostImages { get; set; } = new();
            public Guid ThreadId { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Text).NotEmpty();
                    RuleFor(x => x.PostImages).ForEach(x => x.ValidateImage());
                    RuleFor(x => x.ThreadId).NotEmpty();
                }
            }
        }

        private readonly ThreadboxDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public CreatePost(ThreadboxDbContext dbContext, IFileStorage fileStorage)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Text = request.Text,
                ThreadId = request.ThreadId,
            };

            if (!request.PostImages.Any())
            {
                return Unit.Value;
            }

            foreach (var formFile in request.PostImages)
            {
                SavePostImage(formFile, post.Id, cancellationToken);
            }

            return Unit.Value;
        }

        private async void SavePostImage(IFormFile formFile, Guid postId, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            var data = memoryStream.ToArray();

            await _fileStorage.SaveFileAsync($"Images/PostImages/{postId}/{formFile.Name}", data, cancellationToken);

            // business transaction safety problem
        }
    }
}