using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class ImagesService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly IConfiguration _configuration;

		public ImagesService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_configuration = services.GetRequiredService<IConfiguration>();
		}

		public async Task<Image?> TryGetImageAsync<T>(Guid imageId) where T : Image
		{
			return await _dbContext.FindAsync<T>(imageId);
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