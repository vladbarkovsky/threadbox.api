using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class FileInfoConfiguration : EntityConfigurationTemplate<Domain.Entities.FileInfo>
    {
        protected override void ConfigureConcrete(EntityTypeBuilder<Domain.Entities.FileInfo> builder)
        {
            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}