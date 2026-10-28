using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class DbFileConfiguration : IEntityTypeConfiguration<DbFile>
    {
        public void Configure(EntityTypeBuilder<DbFile> builder)
        {
            builder.HasKey(x => x.Id);
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