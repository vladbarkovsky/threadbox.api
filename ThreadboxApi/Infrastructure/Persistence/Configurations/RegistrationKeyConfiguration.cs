using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class RegistrationKeyConfiguration : BaseEntityConfiguration<RegistrationKey>
    {
        public override void Configure(EntityTypeBuilder<RegistrationKey> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}