using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web;

namespace ThreadboxApi.ORM.Services
{
    public class DbFileStorage : IFileStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public DbFileStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            var data = await _dbContext.DbFiles
                .AsNoTracking()
                .Where(x => x.Path == filePath)
                .Select(x => x.Data)
                .SingleOrDefaultAsync();

            HttpResponseException.ThrowNotFoundIfNull(data);

            return data;
        }

        public Task SaveFileAsync(string path, byte[] data)
        {
            var dbFile = new DbFile
            {
                Path = path,
                Data = data
            };

            _dbContext.DbFiles.Add(dbFile);
            return Task.CompletedTask;
        }

        public async Task DeleteFileAsync(string path)
        {
            var dbFile = await _dbContext.DbFiles
                .Where(x => x.Path == path)
                .SingleOrDefaultAsync();

            HttpResponseException.ThrowNotFoundIfNull(dbFile);
            _dbContext.DbFiles.Remove(dbFile);
        }
    }
}