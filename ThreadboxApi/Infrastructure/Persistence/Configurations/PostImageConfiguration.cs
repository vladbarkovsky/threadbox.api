using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class PostImageConfiguration : BaseEntityConfiguration<PostImage>
    {
        public override void Configure(EntityTypeBuilder<PostImage> builder)
        {
            base.Configure(builder);
        }
    }
}