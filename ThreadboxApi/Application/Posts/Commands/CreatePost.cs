﻿using FluentValidation;
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

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async void SavePostImage(IFormFile formFile, Guid postId, CancellationToken cancellationToken)
        {
            var filePath = $"Images/PostImages/{postId}/{formFile.Name}";

            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            var data = memoryStream.ToArray();

            await _fileStorage.SaveFileAsync(filePath, data, cancellationToken);

            var postImage = new PostImage
            {
                FileInfo = new Domain.Entities.FileInfo
                {
                    Name = formFile.Name,
                    ContentType = formFile.ContentType,
                    Path = filePath
                }
            };

            _dbContext.PostImages.Add(postImage);
        }
    }
}