using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Files.Models;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Files.Queries
{
    public class GetPostImagesZip : IRequestHandler<GetPostImagesZip.Query, FileContentResult>
    {
        public class Query : IRequest<FileContentResult>
        {
            public Guid PostId { get; set; }

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
            var post = await _dbContext.Posts
                .AsNoTracking()
                .Where(x => x.Id == request.PostId)
                .Include(x => x.PostImages)
                .ThenInclude(x => x.FileInfo)
                .FirstOrDefaultAsync(cancellationToken);

            HttpResponseException.ThrowNotFoundIfNull(post);

            var fileInfos = post.PostImages.Select(x => x.FileInfo);
            var fileRetrievalTasks = post.PostImages.Select(x => _fileStorage.GetFileAsync(x.FileInfo.Path));
            var filesData = await Task.WhenAll(fileRetrievalTasks);

            var archivableFiles = fileInfos.Zip(filesData, (fileInfo, data) => new ArchivableFile
            {
                Name = fileInfo.Name,
                Data = data
            });

            var archive = await _zipService.ArchiveAsync(archivableFiles);

            return new FileContentResult(archive, MediaTypeNames.Application.Zip)
            {
                FileDownloadName = $"Post_{post.Id}_images.zip"
            };
        }
    }
}