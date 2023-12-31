﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public class FileInfoConfiguration : BaseEntityConfiguration<Domain.Entities.FileInfo>
    {
        public override void Configure(EntityTypeBuilder<Domain.Entities.FileInfo> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(128);

            builder
                .Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}