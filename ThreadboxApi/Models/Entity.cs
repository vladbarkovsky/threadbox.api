using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Models
{
	/// <summary>
	/// Defines default entity configuration. Override <see cref="Configure(EntityTypeBuilder{TEntity})"/>
	/// to add additional configuration for entity. Don't forget to call base method to apply default
	/// configuration first
	/// </summary>
	public abstract class Entity<TEntity> : IEntityTypeConfiguration<TEntity>
		where TEntity : Entity<TEntity>
	{
		public Guid Id { get; set; }

		public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		{
			builder.HasKey(x => x.Id);
		}
	}
}