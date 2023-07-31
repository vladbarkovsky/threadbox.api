using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public Person Person { get; set; }
    }
}