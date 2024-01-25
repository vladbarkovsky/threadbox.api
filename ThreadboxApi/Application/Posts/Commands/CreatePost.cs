﻿using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common;
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
            public List<IFormFile> PostImages { get; set; }
            public Guid ThreadId { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Text).NotEmpty().MaximumLength(131072);
                    RuleFor(x => x.PostImages).ForEach(x => x.ValidateImage());
                    RuleFor(x => x.ThreadId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public CreatePost(ApplicationDbContext dbContext, IFileStorage fileStorage)
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

            foreach (var formFile in request.PostImages)
            {
                await SavePostImage(formFile, post.Id);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task SavePostImage(IFormFile formFile, Guid postId)
        {
            var filePath = $"Images/PostImages/{postId}/{formFile.Name}";

            using var memoryStream = new MemoryStream();
            formFile.CopyTo(memoryStream);
            var data = memoryStream.ToArray();

            await _fileStorage.SaveFileAsync(filePath, data);

            var postImage = new PostImage
            {
                PostId = postId,
                FileInfo = new ORM.Entities.FileInfo
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