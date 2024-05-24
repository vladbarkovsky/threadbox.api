using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web.Exceptions;

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

            HttpStatusException.ThrowNotFoundIfNull(data);

            return data;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SaveFileAsync(string path, byte[] data)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var dbFile = new DbFile
            {
                Path = path,
                Data = data
            };

            _dbContext.DbFiles.Add(dbFile);
        }

        public async Task DeleteFileAsync(string path)
        {
            var dbFile = await _dbContext.DbFiles
                .Where(x => x.Path == path)
                .SingleOrDefaultAsync();

            HttpStatusException.ThrowNotFoundIfNull(dbFile);
            _dbContext.DbFiles.Remove(dbFile);
        }
    }
}