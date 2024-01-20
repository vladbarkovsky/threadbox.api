using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class ThreadConfiguration : BaseEntityConfiguration<Domain.Entities.Thread>
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.Thread> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .Property(x => x.Text)
                .IsRequired()
                .HasMaxLength(131072);

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
        }
    }
}