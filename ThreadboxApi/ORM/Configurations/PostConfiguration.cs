using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class PostConfiguration : BaseEntityConfiguration<Post>
    {
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Text).IsRequired().HasMaxLength(131072);

            builder
                .HasOne(x => x.Tripcode)
                .WithOne(x => x.Post)
                .HasForeignKey<Post>(x => x.TripcodeId);

            builder
                .HasMany(x => x.PostImages)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .IsRequired();
        }
    }
}