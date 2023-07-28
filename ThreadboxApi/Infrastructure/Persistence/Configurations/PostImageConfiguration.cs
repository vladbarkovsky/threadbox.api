using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class PostImageConfiguration : EntityConfigurationTemplate<PostImage>
    {
        protected override void ConfigureConcrete(EntityTypeBuilder<PostImage> builder)
        {
            builder
                .HasOne(x => x.FileInfo)
                .WithMany(x => x.PostImages)
                .HasForeignKey(x => x.FileInfoId)
                .IsRequired();
        }
    }
}