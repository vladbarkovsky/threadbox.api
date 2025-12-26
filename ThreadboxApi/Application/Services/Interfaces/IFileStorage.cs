using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Application.Services.Interfaces
{
    public interface IFileStorage : IScopedService
    {
        public Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default);

        public Task SaveFileAsync(string path, byte[] data, CancellationToken cancellationToken = default);

        public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
}