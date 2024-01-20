using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Domain.Common
{
    public abstract class BaseEntity : IAuditable, IDeletable
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string UpdatedById { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public bool Deleted { get; set; }
        public byte RowVersion { get; set; }
    }

    public interface IAuditable
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string UpdatedById { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
    }

    public interface IDeletable
    {
        public bool Deleted { get; set; }
    }
}