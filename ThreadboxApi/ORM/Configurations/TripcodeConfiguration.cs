using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class TripcodeConfiguration : BaseEntityConfiguration<Tripcode>
    {
        public override void Configure(EntityTypeBuilder<Tripcode> builder)
        {
            base.Configure(builder);

            builder.HasIndex(x => x.Key);
            builder.Property(x => x.Key).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Salt).IsRequired().HasMaxLength(16);
            builder.Property(x => x.Hash).IsRequired().HasMaxLength(32);

            builder
                .HasMany(x => x.Threads)
                .WithOne(x => x.Tripcode)
                .HasForeignKey(x => x.TripcodeId);

            builder
                .HasMany(x => x.Posts)
                .WithOne(x => x.Tripcode)
                .HasForeignKey(x => x.TripcodeId);
        }
    }
}