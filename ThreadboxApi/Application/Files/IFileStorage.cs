using ThreadboxApi.ServiceLifetimes;

namespace ThreadboxApi.Application.Files
{
    public interface IFileStorage : IScoped
    {
        Task<byte[]> GetFileAsync(string filePath, CancellationToken cancellationToken = default);

        Task SaveFileAsync(string path, byte[] data, CancellationToken cancellationToken = default);

        Task DeleteFilesAsync(CancellationToken cancellationToken = default, params string[] paths);
    }
}