using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Person Person { get; set; }
        public List<AuditLog> AuditLogs { get; set; }
    }
}