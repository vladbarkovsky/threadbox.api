using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Posts.Commands
{
    public class CreatePost : IRequestHandler<CreatePost.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public string Text { get; set; }
            public Guid ThreadId { get; set; }
            public string TripcodeString { get; set; }

            // NOTE: This property will be null in case we pass empty array from client,
            // because we use multipart/form-data encoding for this request.
            public List<IFormFile> PostImages { get; set; } = new();

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Text).NotEmpty().MaximumLength(131072);
                    RuleFor(x => x.ThreadId).NotEmpty();
                    RuleFor(x => x.TripcodeString)
                        .ValidateTripcodeString()
                        .When(x => !string.IsNullOrEmpty(x.TripcodeString));

                    RuleFor(x => x.PostImages)
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
        private readonly TripcodesService _tripcodesService;

        public CreatePost(
            ApplicationDbContext dbContext,
            IFileStorage fileStorage,
            TripcodesService tripcodesService)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
            _tripcodesService = tripcodesService;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Text = request.Text,
                ThreadId = request.ThreadId,
            };

            if (!string.IsNullOrEmpty(request.TripcodeString))
            {
                post.Tripcode = await _tripcodesService.ProcessTripcodeStringAsync(
                    request.TripcodeString,
                    cancellationToken);
            }

            _dbContext.Posts.Add(post);

            foreach (var formFile in request.PostImages)
            {
                await SavePostImage(formFile, post.Id);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task SavePostImage(IFormFile formFile, Guid postId)
        {
            var filePath = $"Images/PostImages/{postId}/{formFile.FileName}";

            using var memoryStream = new MemoryStream();

            // TODO: Use async version of method.
            formFile.CopyTo(memoryStream);

            var data = memoryStream.ToArray();

            await _fileStorage.SaveFileAsync(filePath, data);

            var postImage = new PostImage
            {
                PostId = postId,
                FileInfo = new ORM.Entities.FileInfo
                {
                    Name = formFile.FileName,
                    ContentType = formFile.ContentType,
                    Path = filePath
                }
            };

            _dbContext.PostImages.Add(postImage);
        }
    }
}