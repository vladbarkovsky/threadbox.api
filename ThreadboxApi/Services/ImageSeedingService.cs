using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ThreadboxApi.Services
{
	public class ImageSeedingService : ITransientService
	{
		private readonly IMapper _mapper;

		private List<string> FilePaths { get; }
		private int TotalImages => FilePaths.Count;
		private int Iterator { get; set; }
		private FileExtensionContentTypeProvider FileExtensionContentTypeProvider { get; }

		public ImageSeedingService(IServiceProvider services)
		{
			_mapper = services.GetRequiredService<IMapper>();

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

		public async Task<List<TEntity>> GetImagesForSeeding<TEntity>(int count)
			where TEntity : class, IEntity, IThreadboxFile
		{
			if (Iterator + count > TotalImages)
			{
				throw new ArgumentException("Not enough images.", nameof(count));
			}

			var filePaths = FilePaths.GetRange(Iterator, count);
			Iterator += count;
			var filesAsync = filePaths.Select(x => File.ReadAllBytesAsync(x));
			var files = await Task.WhenAll(filesAsync);
			var images = new List<ThreadboxFile>();

			for (int i = 0; i < filePaths.Count; i++)
			{
				if (!FileExtensionContentTypeProvider.TryGetContentType(filePaths[i], out var contentType))
				{
					throw new Exception($"Can't get Content-Type of {filePaths[i]}.");
				}

				images.Add(new ThreadboxFile
				{
					Name = Path.GetFileName(filePaths[i]),
					Extension = Path.GetExtension(filePaths[i]),
					ContentType = contentType,
					Data = files[i],
				});
			}

			return _mapper.Map<List<TEntity>>(images);
		}
	}
}