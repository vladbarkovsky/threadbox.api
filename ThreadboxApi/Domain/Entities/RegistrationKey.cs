using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class RegistrationKey : BaseEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
    }
}