using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Application.Services.Interfaces
{
    public interface IFileStorage : IScopedService
    {
        Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default);

        Task SaveFileAsync(string path, byte[] data, CancellationToken cancellationToken = default);

        Task DeleteFilesAsync(CancellationToken cancellationToken = default, params string[] paths);
    }
}