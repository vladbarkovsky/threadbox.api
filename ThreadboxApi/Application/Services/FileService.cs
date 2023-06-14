using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Domain.Common;
using ThreadboxApi.Infrastructure.Persistence;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Application.Services
{
    public class FileService : IScopedService
    {
        private readonly Infrastructure.Persistence.DbContext _dbContext;
        private readonly ZipService _zipService;

        public FileService(Infrastructure.Persistence.DbContext dbContext, ZipService zipService)
        {
            _dbContext = dbContext;
            _zipService = zipService;
        }

        public async Task<Domain.Entities.File> GetFileAsync<TEntity>(Guid fileEntityId)
            where TEntity : FileEntity<TEntity>
        {
            var file = await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .Where(x => x.Id == fileEntityId)
                .Select(x => x.File)
                .FirstOrDefaultAsync();

            HttpResponseException.ThrowNotFoundIfNull(file);
            return file!;
        }

        public async Task<byte[]> GetThreadImagesAsync(Guid threadId)
        {
            var images = await _dbContext.ThreadImages
                .AsNoTracking()
                .Where(x => x.ThreadId == threadId)
                .Select(x => x.File)
                .ToListAsync();

            if (!images.Any())
            {
                throw HttpResponseException.NotFound;
            }

            return await _zipService.ArchiveAsync(images);
        }

        public async Task<byte[]> GetPostImagesAsync(Guid postId)
        {
            var images = await _dbContext.PostImages
                .AsNoTracking()
                .Where(x => x.PostId == postId)
                .Select(x => x.File)
                .ToListAsync();

            if (!images.Any())
            {
                throw HttpResponseException.NotFound;
            }

            return await _zipService.ArchiveAsync(images);
        }
    }
}