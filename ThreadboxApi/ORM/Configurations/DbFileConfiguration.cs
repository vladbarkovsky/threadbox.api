using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class DbFileConfiguration : BaseEntityConfiguration<DbFile>
    {
        public override void Configure(EntityTypeBuilder<DbFile> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Path).IsUnique();

            builder
                .Property(x => x.Path)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .Property(x => x.Data)
                .IsRequired()
                // 10 MB
                .HasMaxLength(10 * 1024 * 1024);
        }
    }
}