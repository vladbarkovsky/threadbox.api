using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class ImagesService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly ZipService _zipService;
		private readonly IMapper _mapper;

		public ImagesService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_zipService = services.GetRequiredService<ZipService>();
			_mapper = services.GetRequiredService<IMapper>();
		}

		public async Task<Image?> TryGetImageAsync<T>(Guid imageId) where T : Image
		{
			return await _dbContext.FindAsync<T>(imageId);
		}

		public async Task<byte[]?> TryGetThreadImagesAsync(Guid threadId)
		{
			var images = await _dbContext.ThreadImages
				.Where(x => x.ThreadId == threadId)
				.ToListAsync();

			if (!images.Any())
			{
				return null;
			}

			var files = _mapper.Map<List<ThreadboxFile>>(images);
			return await _zipService.ArchiveAsync(files);
		}

		public async Task<byte[]?> TryGetPostImagesAsync(Guid postId)
		{
			var images = await _dbContext.PostImages
				.Where(x => x.PostId == postId)
				.ToListAsync();

			if (!images.Any())
			{
				return null;
			}

			var files = _mapper.Map<List<ThreadboxFile>>(images);
			return await _zipService.ArchiveAsync(files);
		}

		public async Task<List<Image>> GetImagesForSeeding(int index, int count)
		{
			// TODO: Implement normal files ordering (now is 0, 1, 10, 100...)
			var filePaths = Directory.EnumerateFiles(Constants.TestImagesDirectory).ToList();

			// TODO: Arguments validation + argument exceptions

			filePaths = filePaths.GetRange(index, count).ToList();
			var filesAsync = filePaths.Select(x => File.ReadAllBytesAsync(x));
			var files = await Task.WhenAll(filesAsync);
			List<Image> images = new();

			for (int i = 0; i < filePaths.Count; i++)
			{
				images.Add(new Image
				{
					Data = files[i],
					Extension = Path.GetExtension(filePaths[i]).TrimStart('.')
				});
			}

			return images;
		}
	}
}