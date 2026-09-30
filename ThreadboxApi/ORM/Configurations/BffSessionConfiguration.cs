using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class BffSessionConfiguration : IEntityTypeConfiguration<BffSession>
    {
        public void Configure(EntityTypeBuilder<BffSession> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AccessToken).IsRequired();
            builder.Property(x => x.RefreshToken).IsRequired();
        }
    }
}
