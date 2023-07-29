using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Application.Files.Models;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Files.Queries
{
    public class GetTreadImagesZip : IRequestHandler<GetTreadImagesZip.Query, FileContentResult>
    {
        public class Query : IRequest<FileContentResult>
        {
            public Guid ThreadId { get; set; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.ThreadId).NotEmpty();
                }
            }
        }

        private readonly AppDbContext _dbContext;
        private readonly IFileStorage _fileStorage;
        private readonly ZipService _zipService;

        public GetTreadImagesZip(AppDbContext dbContext, IFileStorage fileStorage, ZipService zipService)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
            _zipService = zipService;
        }

        public async Task<FileContentResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var threads = await _dbContext.Threads
                .AsNoTracking()
                .Where(x => x.Id == request.ThreadId)
                .Include(x => x.ThreadImages)
                .ThenInclude(x => x.FileInfo)
                .FirstOrDefaultAsync(cancellationToken);

            HttpResponseException.ThrowNotFoundIfNull(threads);

            var fileInfos = threads.ThreadImages.Select(x => x.FileInfo);
            var fileRetrievalTasks = threads.ThreadImages.Select(x => _fileStorage.GetFileAsync(x.FileInfo.Path, cancellationToken));
            var filesData = await Task.WhenAll(fileRetrievalTasks);

            var archivableFiles = fileInfos.Zip(filesData, (fileInfo, data) => new ArchivableFile
            {
                Name = fileInfo.Name,
                Data = data
            });

            var archive = await _zipService.ArchiveAsync(archivableFiles);

            return new FileContentResult(archive, MediaTypeNames.Application.Zip)
            {
                FileDownloadName = $"Thread_{threads.Id}_images.zip"
            };
        }
    }
}