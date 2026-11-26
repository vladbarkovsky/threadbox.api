using System.IO.Compression;
using ThreadboxApi.Application.Files.Models;
using ThreadboxApi.ServiceLifetimes;

namespace ThreadboxApi.Application.Files
{
    public class ZipService : IScoped
    {
        // Source: https://swimburger.net/blog/dotnet/create-zip-files-on-http-request-without-intermediate-files-using-aspdotnet-mvc-framework
        public async Task<byte[]> ArchiveAsync(IEnumerable<ArchivableFile> archivableFiles)
        {
            var archiveStream = new MemoryStream();
            var copyOperationsAsync = new List<Task>();

            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (ArchivableFile file in archivableFiles)
                {
                    ZipArchiveEntry entry = archive.CreateEntry(file.Name);

                    using Stream entryStream = entry.Open();
                    using var fileStream = new MemoryStream(file.Data);
                    copyOperationsAsync.Add(fileStream.CopyToAsync(entryStream));
                }
            }

            await Task.WhenAll(copyOperationsAsync);
            archiveStream.Seek(0, SeekOrigin.Begin);
            return archiveStream.ToArray();
        }
    }
}