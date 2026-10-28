using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using ThreadboxApi.Application.Files.Models;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Threads.Queries
{
    public class GetThreadImagesZip : IRequestHandler<GetThreadImagesZip.Query, FileContentResult>
    {
        public class Query : IRequest<FileContentResult>
        {
            public Guid ThreadId { get; init; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.ThreadId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;
        private readonly ZipService _zipService;

        public GetThreadImagesZip(ApplicationDbContext dbContext, IFileStorage fileStorage, ZipService zipService)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
            _zipService = zipService;
        }

        public async Task<FileContentResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var thread = await _dbContext.Threads
                .AsNoTracking()
                .Where(x => x.Id == request.ThreadId)
                .Include(x => x.ThreadImages)
                .ThenInclude(x => x.FileInfo)
                .SingleOrDefaultAsync(cancellationToken);

            if (thread == null)
            {
                throw new HttpResponseException($"Thread with ID \"{request.ThreadId}\" not found.", StatusCodes.Status404NotFound);
            }

            IEnumerable<ORM.Entities.FileInfo> fileInfos = thread.ThreadImages.Select(x => x.FileInfo);
            var files = new List<byte[]>();

            foreach (var fileInfo in fileInfos)
            {
                files.Add(await _fileStorage.GetFileAsync(fileInfo.Path, cancellationToken));
            }

            IEnumerable<ArchivableFile> archivableFiles = fileInfos.Zip(
                second: files,
                resultSelector: ArchivableFile (ORM.Entities.FileInfo fileInfo, byte[] file) => new ArchivableFile
                {
                    Name = fileInfo.Name,
                    Data = file
                });

            byte[] archive = await _zipService.ArchiveAsync(archivableFiles);

            return new FileContentResult(archive, MediaTypeNames.Application.Zip)
            {
                FileDownloadName = $"Thread_{thread.Id}_images.zip"
            };
        }
    }
}