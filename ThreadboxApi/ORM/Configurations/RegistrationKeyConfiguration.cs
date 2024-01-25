using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class RegistrationKeyConfiguration : BaseEntityConfiguration<RegistrationKey>
    {
        public override void Configure(EntityTypeBuilder<RegistrationKey> builder)
        {
            base.Configure(builder);
        }
    }
}