using ThreadboxApi.Application.Common.Interfaces;

namespace ThreadboxApi.Application.Files.Interfaces
{
    public interface IFileStorage : IScopedService
    {
        public Task<byte[]> GetFileAsync(string filePath);

        public Task SaveFileAsync(string path, byte[] data);

        public Task DeleteFileAsync(string filePath);
    }
}