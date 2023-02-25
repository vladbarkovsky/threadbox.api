using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;

namespace ThreadboxApi.Services
{
	public class ImagesService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly ZipService _zipService;

		public ImagesService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_zipService = services.GetRequiredService<ZipService>();
		}

		public async Task<byte[]?> TryGetThreadImagesAsync(Guid threadId)
		{
			var images = await _dbContext.ThreadImages
				.AsNoTracking()
				.Where(x => x.ThreadId == threadId)
				.Select(x => x.File)
				.ToListAsync();

			if (!images.Any())
			{
				return null;
			}

			return await _zipService.ArchiveAsync(images);
		}

		public async Task<byte[]?> TryGetPostImagesAsync(Guid postId)
		{
			var images = await _dbContext.PostImages
				.AsNoTracking()
				.Where(x => x.PostId == postId)
				.Select(x => x.File)
				.ToListAsync();

			if (!images.Any())
			{
				return null;
			}

			return await _zipService.ArchiveAsync(images);
		}
	}
}