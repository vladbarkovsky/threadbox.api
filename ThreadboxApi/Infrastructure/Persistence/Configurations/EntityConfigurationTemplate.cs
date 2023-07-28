using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Infrastructure.Persistence.Configurations
{
    public abstract class EntityConfigurationTemplate<T> : IEntityTypeConfiguration<T>
        where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            Configure(builder);
        }

        protected virtual void ConfigureConcrete(EntityTypeBuilder<T> builder)
        { }
    }
}