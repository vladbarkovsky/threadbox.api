using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class PostImageConfiguration : FileEntityConfiguration<PostImage>
    {
        public override void Configure(EntityTypeBuilder<PostImage> builder)
        {
            base.Configure(builder);
        }
    }
}