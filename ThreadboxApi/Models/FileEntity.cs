using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Models
{
	public abstract class FileEntity<TEntity> : Entity<TEntity>
		where TEntity : FileEntity<TEntity>
	{
		public ThreadboxFile File { get; set; } = null!;

		public override void Configure(EntityTypeBuilder<TEntity> builder)
		{
			base.Configure(builder);
			builder.OwnsOne(x => x.File);
		}
	}
}