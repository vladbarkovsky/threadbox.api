using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class BoardConfiguration : BaseEntityConfiguration<Board>
    {
        public override void Configure(EntityTypeBuilder<Board> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.Description).HasMaxLength(2048);

            builder
                .HasMany(x => x.Threads)
                .WithOne(x => x.Board)
                .HasForeignKey(x => x.BoardId)
                .IsRequired();
        }
    }
}