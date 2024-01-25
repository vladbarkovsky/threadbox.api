using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class PostImageConfiguration : BaseEntityConfiguration<PostImage>
    {
        public override void Configure(EntityTypeBuilder<PostImage> builder)
        {
            base.Configure(builder);
        }
    }
}