using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Services
{
	public class FilesService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly ZipService _zipService;

		public FilesService(ThreadboxDbContext dbContext, ZipService zipService)
		{
			_dbContext = dbContext;
			_zipService = zipService;
		}

		public async Task<ThreadboxFile> GetFileAsync<TEntity>(Guid fileEntityId)
			where TEntity : FileEntity<TEntity>
		{
			var file = await _dbContext.Set<TEntity>()
				.AsNoTracking()
				.Where(x => x.Id == fileEntityId)
				.Select(x => x.File)
				.FirstOrDefaultAsync();

			HttpResponseExceptions.ThrowNotFoundIfNull(file);
			return file!;
		}

		public async Task<byte[]> GetThreadImagesAsync(Guid threadId)
		{
			var images = await _dbContext.ThreadImages
				.AsNoTracking()
				.Where(x => x.ThreadId == threadId)
				.Select(x => x.File)
				.ToListAsync();

			if (!images.Any())
			{
				throw HttpResponseExceptions.NotFound;
			}

			return await _zipService.ArchiveAsync(images);
		}

		public async Task<byte[]> GetPostImagesAsync(Guid postId)
		{
			var images = await _dbContext.PostImages
				.AsNoTracking()
				.Where(x => x.PostId == postId)
				.Select(x => x.File)
				.ToListAsync();

			if (!images.Any())
			{
				throw HttpResponseExceptions.NotFound;
			}

			return await _zipService.ArchiveAsync(images);
		}
	}
}