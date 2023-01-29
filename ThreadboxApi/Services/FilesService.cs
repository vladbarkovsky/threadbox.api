using AutoMapper;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class FilesService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly IMapper _mapper;

		public FilesService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_mapper = services.GetRequiredService<IMapper>();
		}

		public async Task<ThreadboxFile?> TryGetFileAsync<TEntity>(Guid entityId)
			where TEntity : class, IEntity, IThreadboxFile
		{
			var entity = await _dbContext.FindAsync<TEntity>(entityId);
			return entity != null ? _mapper.Map<ThreadboxFile>(entity) : null;
		}
	}
}