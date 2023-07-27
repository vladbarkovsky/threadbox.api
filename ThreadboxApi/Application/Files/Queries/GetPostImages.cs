using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Application.Files.Models;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Files.Queries
{
    public class GetPostImages : IRequestHandler<GetPostImages.Query, FileContentResult>
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

        private readonly AppDbContext _dbContext;
        private readonly IFileStorage _fileStorage;
        private readonly ZipService _zipService;

        public GetPostImages(AppDbContext dbContext, IFileStorage fileStorage, ZipService zipService)
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

            var archive = _zipService.ArchiveAsync(archivableFiles);
        }

        private async Task<ArchivableFile> GetFileForArchivation(IEnumerable<Domain.Entities.FileInfo> fileInfos, CancellationToken cancellationToken)
        {
            var data = await _fileStorage.GetFileAsync(fileInfo.Path, cancellationToken);
            return new ArchivableFile
            {
                Name = fileInfo.Name,
                Data = data
            };
        }
    }
}