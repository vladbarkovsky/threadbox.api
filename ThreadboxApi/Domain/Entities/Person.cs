using ThreadboxApi.Domain.Common;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}