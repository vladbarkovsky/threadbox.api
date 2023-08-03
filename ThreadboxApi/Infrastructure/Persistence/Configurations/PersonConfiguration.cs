using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : BaseEntityConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(x => x.User)
                .WithOne(x => x.Person)
                .HasForeignKey<Person>(x => x.UserId)
                .IsRequired();
        }
    }
}