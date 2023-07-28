using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class ThreadConfiguration : EntityConfigurationTemplate<Domain.Entities.Thread>
    {
        protected override void ConfigureConcrete(EntityTypeBuilder<Domain.Entities.Thread> builder)
        {
            builder
                .HasMany(x => x.Posts)
                .WithOne(x => x.Thread)
                .HasForeignKey(x => x.ThreadId)
                .IsRequired();

            builder
                .HasMany(x => x.ThreadImages)
                .WithOne(x => x.Thread)
                .HasForeignKey(x => x.ThreadId)
                .IsRequired();

            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(131072);
        }
    }
}