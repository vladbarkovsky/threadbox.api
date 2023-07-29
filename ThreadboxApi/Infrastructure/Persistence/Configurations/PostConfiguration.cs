using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class PostConfiguration : EntityConfigurationTemplate<Post>
    {
        protected override void OnConfiguring(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasMany(x => x.PostImages)
                .WithOne(x => x.Post)
                .HasForeignKey(x => x.PostId)
                .IsRequired();

            builder
                .Property(x => x.Text)
                .IsRequired(false)
                .HasMaxLength(131072);
        }
    }
}