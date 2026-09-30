using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.ORM.Services
{
    public class DbFileStorage : IFileStorage
    {
        private readonly ApplicationDbContext _dbContext;

        public DbFileStorage(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<byte[]> GetFileAsync(string path, CancellationToken cancellationToken)
        {
            byte[] fileData = await _dbContext.DbFiles
                .AsNoTracking()
                .Where(bool (DbFile dbFile) => dbFile.Path == path)
                .Select(byte[] (DbFile dbFile) => dbFile.Data)
                .SingleOrDefaultAsync(cancellationToken);

            if (fileData == null)
            {
                throw new HttpResponseException($"Data of file with path \"{path}\" not found.", StatusCodes.Status404NotFound);
            }

            return fileData;
        }

        public Task SaveFileAsync(string path, byte[] data, CancellationToken cancellationToken)
        {
            var dbFile = new DbFile
            {
                Path = path,
                Data = data
            };

            _dbContext.DbFiles.Add(dbFile);
            return Task.CompletedTask;
        }

        public async Task DeleteFileAsync(string path, CancellationToken cancellationToken)
        {
            DbFile dbFile = await _dbContext.DbFiles
                .Where(bool (DbFile dbFile) => dbFile.Path == path)
                .SingleOrDefaultAsync(cancellationToken);

            if (dbFile == null)
            {
                throw new HttpResponseException($"File with path \"{path}\" not found.", StatusCodes.Status404NotFound);
            }

            _dbContext.DbFiles.Remove(dbFile);
        }
    }
}
