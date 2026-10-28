using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using ThreadboxApi.Application.Files.Models;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Files.Queries
{
    public class GetPostImagesZip : IRequestHandler<GetPostImagesZip.Query, FileContentResult>
    {
        public class Query : IRequest<FileContentResult>
        {
            public Guid PostId { get; init; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.PostId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;
        private readonly ZipService _zipService;

        public GetPostImagesZip(ApplicationDbContext dbContext, IFileStorage fileStorage, ZipService zipService)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
            _zipService = zipService;
        }

        public async Task<FileContentResult> Handle(Query request, CancellationToken cancellationToken)
        {
            Post post = await _dbContext.Posts
                .AsNoTracking()
                .Where(x => x.Id == request.PostId)
                .Include(x => x.PostImages)
                .ThenInclude(x => x.FileInfo)
                .SingleOrDefaultAsync(cancellationToken);

            if (post == null)
            {
                throw new HttpResponseException($"Post with ID \"{request.PostId}\" not found.", StatusCodes.Status404NotFound);
            }

            IEnumerable<ORM.Entities.FileInfo> fileInfos = post.PostImages.Select(ORM.Entities.FileInfo (PostImage image) => image.FileInfo);
            var files = new List<byte[]>();

            foreach (ORM.Entities.FileInfo fileInfo in fileInfos)
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
                FileDownloadName = $"Post_{post.Id}_images.zip"
            };
        }
    }
}