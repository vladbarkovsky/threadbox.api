using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class ThreadImageConfiguration : EntityConfigurationTemplate<ThreadImage>
    {
        protected override void ConfigureConcrete(EntityTypeBuilder<ThreadImage> builder)
        {
            builder
                .HasOne(x => x.FileInfo)
                .WithMany(x => x.ThreadImages)
                .HasForeignKey(x => x.FileInfoId)
                .IsRequired();
        }
    }
}