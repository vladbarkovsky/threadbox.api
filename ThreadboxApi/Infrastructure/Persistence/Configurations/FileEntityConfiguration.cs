using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class FileEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity>
        where TEntity : FileEntity<TEntity>
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            builder.OwnsOne(x => x.File);
        }
    }
}