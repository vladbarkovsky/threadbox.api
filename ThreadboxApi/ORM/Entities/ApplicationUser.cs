using Microsoft.AspNetCore.Identity;

namespace ThreadboxApi.ORM.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Person Person { get; set; }
        public List<AuditLog> AuditLogs { get; set; }
    }
}