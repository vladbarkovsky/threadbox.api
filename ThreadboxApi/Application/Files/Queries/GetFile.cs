using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

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

            if (fileInfo == null)
            {
                throw new HttpResponseException($"File info with ID \"{request.FileInfoId}\" not found.", StatusCodes.Status404NotFound);
            }

            var data = await _fileStorage.GetFileAsync(fileInfo.Path);

            return new FileContentResult(data, fileInfo.ContentType)
            {
                FileDownloadName = fileInfo.Name
            };
        }
    }
}