using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
    }
}