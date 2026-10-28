using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.ORM.Entities
{
    public abstract class BaseEntity : IConsistent, IDeletable, IAuditable
    {
        public Guid Id { get; set; }
        public uint RowVersion { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedById { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string UpdatedById { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
    }
}