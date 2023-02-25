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

		public FilesService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
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
	}
}