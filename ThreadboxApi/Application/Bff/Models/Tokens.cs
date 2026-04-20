namespace ThreadboxApi.Application.Bff.Models
{
    public class Tokens
    {
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public string RefreshToken { get; set; }
    }
}
