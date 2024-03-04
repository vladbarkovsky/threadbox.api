using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class FileInfoConfiguration : BaseEntityConfiguration<Entities.FileInfo>
    {
        public override void Configure(EntityTypeBuilder<Entities.FileInfo> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder
                .Property(x => x.ContentType)
                // According to RFC 4288 page 6.
                // https://datatracker.ietf.org/doc/html/rfc4288
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(x => x.Path)
                .IsRequired()
                .HasMaxLength(256);

            builder
                .HasOne(x => x.ThreadImage)
                .WithOne(x => x.FileInfo)
                .HasForeignKey<ThreadImage>(x => x.FileInfoId)
                .IsRequired();

            builder
                .HasOne(x => x.PostImage)
                .WithOne(x => x.FileInfo)
                .HasForeignKey<PostImage>(x => x.FileInfoId)
                .IsRequired();
        }
    }
}