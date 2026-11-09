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
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            byte[] fileData = await _dbContext.DbFiles
                .AsNoTracking()
                .Where(x => x.Path == path)
                .Select(x => x.Data)
                .SingleOrDefaultAsync(cancellationToken);

            if (fileData == null)
            {
                throw new HttpResponseException($"Data of file with path \"{path}\" not found.", StatusCodes.Status404NotFound);
            }

            return fileData;
        }

        public Task SaveFileAsync(string path, byte[] data, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            }

            if (data.Length == 0)
            {
                throw new ArgumentException("Data cannot be empty.", nameof(data));
            }

            var dbFile = new DbFile
            {
                Path = path,
                Data = data
            };

            _dbContext.DbFiles.Add(dbFile);
            return Task.CompletedTask;
        }

        public async Task DeleteFilesAsync(CancellationToken cancellationToken, params string[] paths)
        {
            if (paths == null || paths.Length == 0)
            {
                throw new ArgumentException("Paths cannot be null or empty.", nameof(paths));
            }

            if (paths.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("File paths cannot contain null or empty values.", nameof(paths));
            }

            DbFile[] dbFiles = await _dbContext.DbFiles
                .Where(x => paths.Contains(x.Path))
                .ToArrayAsync(cancellationToken);

            _dbContext.DbFiles.RemoveRange(dbFiles);
        }
    }
}
