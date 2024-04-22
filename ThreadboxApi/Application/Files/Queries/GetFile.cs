using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web;

namespace ThreadboxApi.Application.Files.Queries
{
    public class GetFile : IRequestHandler<GetFile.Query, FileContentResult>
    {
        public class Query : IRequest<FileContentResult>
        {
            public Guid FileInfoId { get; set; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.FileInfoId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public GetFile(ApplicationDbContext dbContext, IFileStorage fileStorage)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
        }

        public async Task<FileContentResult> Handle(Query request, CancellationToken cancellationToken)
        {
            var fileInfo = await _dbContext.FileInfos
                .AsNoTracking()
                .Where(x => x.Id == request.FileInfoId)
                .FirstOrDefaultAsync(cancellationToken);

            HttpResponseException.ThrowNotFoundIfNull(fileInfo);
            var data = await _fileStorage.GetFileAsync(fileInfo.Path);

            return new FileContentResult(data, fileInfo.ContentType)
            {
                FileDownloadName = fileInfo.Name
            };
        }
    }
}