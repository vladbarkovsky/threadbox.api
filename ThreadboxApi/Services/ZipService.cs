using System.IO.Compression;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class ZipService : IScopedService
	{
		// Source: https://swimburger.net/blog/dotnet/create-zip-files-on-http-request-without-intermediate-files-using-aspdotnet-mvc-framework
		public async Task<byte[]> ArchiveAsync(IEnumerable<IThreadboxFile> files)
		{
			var archiveStream = new MemoryStream();
			var copyOperationsAsync = new List<Task>();

			using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
			{
				foreach (var file in files)
				{
					var entry = archive.CreateEntry(file.Name);

					using var entryStream = entry.Open();
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