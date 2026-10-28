using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Controllers;

namespace ThreadboxApi.Application.Posts.Commands
{
    public class CreatePost : IRequestHandler<CreatePost.Command, CreatedAtActionResult>
    {
        public class Command : IRequest<CreatedAtActionResult>
        {
            public string Text { get; init; }
            public Guid ThreadId { get; init; }
            public string TripcodeString { get; init; }

            // NOTE: This property will be null in case we pass empty array from client,
            // because we use multipart/form-data encoding for this request.
            public IFormFile[] PostImages { get; init; } = Array.Empty<FormFile>();

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
                        .Must(x => x.Length <= 5)
                        .WithMessage("Maximum allowed number of files is 5.")
                        .ForEach(x => x.ValidateImage())
                        .WithUnique(x => x.FileName);
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;
        private readonly TripcodesService _tripcodesService;
        private readonly IMapper _mapper;

        public CreatePost(
            ApplicationDbContext dbContext,
            IFileStorage fileStorage,
            TripcodesService tripcodesService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
            _tripcodesService = tripcodesService;
            _mapper = mapper;
        }

        public async Task<CreatedAtActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Id = Guid.NewGuid(),
                Text = request.Text,
                ThreadId = request.ThreadId,
            };

            if (!string.IsNullOrEmpty(request.TripcodeString))
            {
                post.Tripcode = await _tripcodesService.ProcessTripcodeStringAsync(request.TripcodeString, cancellationToken);
            }

            _dbContext.Posts.Add(post);

            foreach (IFormFile formFile in request.PostImages)
            {
                await SavePostImage(formFile, post.Id, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedAtActionResult(
                actionName: nameof(PostController.Get),
                controllerName: nameof(PostController),
                routeValues: new { id = post.Id },
                value: _mapper.Map<PostDto>(post));
        }

        private async Task SavePostImage(IFormFile formFile, Guid postId, CancellationToken cancellationToken)
        {
            var filePath = $"Images/PostImages/{postId}/{formFile.FileName}";

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream, cancellationToken);
                byte[] data = memoryStream.ToArray();
                await _fileStorage.SaveFileAsync(filePath, data, cancellationToken);
            }

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