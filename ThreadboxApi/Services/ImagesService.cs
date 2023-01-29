using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
	}
}