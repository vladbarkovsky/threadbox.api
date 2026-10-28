namespace ThreadboxApi.Application.Identity.Models
{
    public class UserSession
    {
        public string Username { get; init; }
        public IEnumerable<string> Permissions { get; init; }
    }
}
