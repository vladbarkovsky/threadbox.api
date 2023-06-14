using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class ThreadImageConfiguration : FileEntityConfiguration<ThreadImage>
    {
        public override void Configure(EntityTypeBuilder<ThreadImage> builder)
        {
            base.Configure(builder);
        }
    }
}