using Microsoft.AspNetCore.StaticFiles;
using System.Text.RegularExpressions;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	/// <summary>
	/// Generates file entities for seeding
	/// </summary>
	public class FileSeedingService : ITransientService
	{
		private List<string> FilePaths { get; }
		private int TotalFiles => FilePaths.Count;
		private int Iterator { get; set; }
		private FileExtensionContentTypeProvider FileExtensionContentTypeProvider { get; }

		public FileSeedingService(IServiceProvider services)
		{
			FilePaths = Directory
				.GetFiles(Constants.TestImagesDirectory)
				// Pad each match of numeric values in the file path with a 0 character to a length of 4 characters.
				// This is necessary to preserve the order of files containing a serial number in the name.
				// WARNING: tested only on NTFS
				// Taken from https://stackoverflow.com/a/46143251
				.OrderBy(filePath => Regex.Replace(filePath, @"\d+", match => match.Value.PadLeft(4, '0')))
				.ToList();

			FileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
		}

		/// <summary>
		/// Generates defined count of file entities.
		/// Maximum total count of entities per service injection is equal to count of files
		/// in directory used for files seeding
		/// </summary>
		/// <typeparam name="TEntity">Type of file entities you want to generate</typeparam>
		/// <param name="count">Count of file entities you want to generate</param>
		/// <returns>List of file entities</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="Exception"></exception>
		public async Task<List<TEntity>> GetFilesForSeeding<TEntity>(int count)
			where TEntity : FileEntity<TEntity>
		{
			if (Iterator + count > TotalFiles)
			{
				throw new ArgumentException("Not enough files.", nameof(count));
			}

			var filePaths = FilePaths.GetRange(Iterator, count);
			Iterator += count;
			var filesDataAsync = filePaths.Select(x => File.ReadAllBytesAsync(x));
			var filesData = await Task.WhenAll(filesDataAsync);
			var files = new List<ThreadboxFile>();

			for (int i = 0; i < filePaths.Count; i++)
			{
				if (!FileExtensionContentTypeProvider.TryGetContentType(filePaths[i], out var contentType))
				{
					throw new Exception($"Can't get Content-Type of {filePaths[i]}.");
				}

				files.Add(new ThreadboxFile
				{
					Name = Path.GetFileName(filePaths[i]),
					Extension = Path.GetExtension(filePaths[i]),
					ContentType = contentType,
					Data = filesData[i],
				});
			}

			return files
				.Select(x =>
				{
					var entity = Activator.CreateInstance<TEntity>();
					entity.File = x;
					return entity;
				})
				.ToList();
		}
	}
}