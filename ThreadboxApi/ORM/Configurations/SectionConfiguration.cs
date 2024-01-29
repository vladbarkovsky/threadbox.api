using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.ORM.Configurations
{
    public class SectionConfiguration : BaseEntityConfiguration<Section>
    {
        public override void Configure(EntityTypeBuilder<Section> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .HasMany(x => x.Boards)
                .WithOne(x => x.Section)
                .HasForeignKey(x => x.SectionId);
        }
    }
}