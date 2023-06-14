using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class RegistrationKey : BaseEntity<RegistrationKey>
    {
        public DateTimeOffset CreatedAt { get; set; }
    }
}