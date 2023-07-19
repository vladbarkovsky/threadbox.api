using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Services
{
    public class DbFileStorage : IFileStorage
    {
        private readonly ThreadboxDbContext _dbContext;

        public DbFileStorage(ThreadboxDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken)
        {
            var data = await _dbContext.DbFiles
                .AsNoTracking()
                .Where(x => x.Path == filePath)
                .Select(x => x.Data)
                .FirstOrDefaultAsync(cancellationToken);

            HttpResponseException.ThrowNotFoundIfNull(data);

            return data;
        }

        public async Task SaveFileAsync(string path, byte[] data, CancellationToken cancellationToken)
        {
            var dbFile = new DbFile
            {
                Path = path,
                Data = data
            };

            using var transaction = _dbContext.Database.BeginTransaction();
            _dbContext.DbFiles.Add(dbFile);
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task DeleteFileAsync(string path, CancellationToken cancellationToken)
        {
            var dbFile = await _dbContext.DbFiles
                .Where(x => x.Path == path)
                .FirstOrDefaultAsync(cancellationToken);

            HttpResponseException.ThrowNotFoundIfNull(dbFile);

            using var transaction = _dbContext.Database.BeginTransaction();
            _dbContext.DbFiles.Remove(dbFile);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}