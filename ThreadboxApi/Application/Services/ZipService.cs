using System.IO.Compression;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Application.Files.Models;

namespace ThreadboxApi.Application.Services
{
    public class ZipService : IScopedService
    {
        // Source: https://swimburger.net/blog/dotnet/create-zip-files-on-http-request-without-intermediate-files-using-aspdotnet-mvc-framework
        public async Task<byte[]> ArchiveAsync(IEnumerable<ArchivableFile> archivableFiles)
        {
            var archiveStream = new MemoryStream();
            var copyOperationsAsync = new List<Task>();

            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var file in archivableFiles)
                {
                    var entry = archive.CreateEntry(file.Name);

                    using var entryStream = entry.Open();
                    using var fileStream = new MemoryStream(file.Data);
                    copyOperationsAsync.Add(fileStream.CopyToAsync(entryStream));
                }
            }

            await Task.WhenAll(copyOperationsAsync);
            archiveStream.Seek(0, SeekOrigin.Begin);
            return archiveStream.ToArray()
        }
    }
}