using System.ComponentModel.DataAnnotations;

namespace ThreadboxApi.Application.Identity.Models
{
    public class UserSessionDto
    {
        [Required]
        public string Username { get; init; }

        [Required]
        public IEnumerable<string> Permissions { get; init; }
    }
}
