using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Controllers;

namespace ThreadboxApi.Application.Threads.Commands
{
    public class CreateThread : IRequestHandler<CreateThread.Command, CreatedAtActionResult>
    {
        public class Command : IRequest<CreatedAtActionResult>
        {
            public Guid BoardId { get; init; }
            public string Title { get; init; }
            public string Text { get; init; }
            public string TripcodeString { get; init; }

            // NOTE: This property will be null in case we pass empty array from client,
            // because we use multipart/form-data encoding for this request.
            public IFormFile[] Images { get; init; } = Array.Empty<IFormFile>();

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.Title).NotEmpty().MaximumLength(128);
                    RuleFor(x => x.Text).NotEmpty().MaximumLength(131072);
                    RuleFor(x => x.BoardId).NotEmpty();
                    RuleFor(x => x.TripcodeString)
                        .ValidateTripcodeString()
                        .When(x => !string.IsNullOrEmpty(x.TripcodeString));
                    RuleFor(x => x.Images)
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

        public CreateThread(
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
            var thread = new ORM.Entities.Thread()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Text = request.Text,
                BoardId = request.BoardId,
            };

            if (!string.IsNullOrEmpty(request.TripcodeString))
            {
                thread.Tripcode = await _tripcodesService.ProcessTripcodeStringAsync(request.TripcodeString, cancellationToken);
            }

            _dbContext.Add(thread);

            foreach (IFormFile threadImage in request.Images)
            {
                await SaveThreadImage(threadImage, thread.Id, cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedAtActionResult(
                actionName: nameof(ThreadController.Get),
                controllerName: nameof(ThreadController),
                routeValues: new { id = thread.Id },
                value: _mapper.Map<ThreadDto>(thread));
        }

        private async Task SaveThreadImage(IFormFile formFile, Guid threadId, CancellationToken cancellationToken)
        {
            var filePath = $"Images/ThreadImages/{threadId}/{formFile.FileName}";

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream, cancellationToken);
                byte[] data = memoryStream.ToArray();
                await _fileStorage.SaveFileAsync(filePath, data, cancellationToken);
            }

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