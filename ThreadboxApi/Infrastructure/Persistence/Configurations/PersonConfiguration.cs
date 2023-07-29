using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class PersonConfiguration : EntityConfigurationTemplate<Person>
    {
        protected override void OnConfiguring(EntityTypeBuilder<Person> builder)
        {
            builder
                .HasOne(x => x.AppUser)
                .WithOne(x => x.Person)
                .HasForeignKey<Person>(x => x.AppUserId)
                .IsRequired();
        }
    }
}