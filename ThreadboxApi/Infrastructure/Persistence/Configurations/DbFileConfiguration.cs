using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class DbFileConfiguration : EntityConfigurationTemplate<DbFile>
    {
        protected override void ConfigureConcrete(EntityTypeBuilder<DbFile> builder)
        {
            builder.HasIndex(x => x.Path).IsUnique();

            builder
                .Property(x => x.Path)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .Property(x => x.Data)
                .IsRequired()
                .HasMaxLength(10 * 1024 * 1024);
        }
    }
}