using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class ThreadImageConfiguration : BaseEntityConfiguration<ThreadImage>
    {
        public override void Configure(EntityTypeBuilder<ThreadImage> builder)
        {
            base.Configure(builder);
        }
    }
}