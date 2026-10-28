namespace ThreadboxApi.Application.Bff.Models
{
    public class Tokens
    {
        public string AccessToken { get; init; }
        public DateTimeOffset ExpiresAt { get; init; }
        public string RefreshToken { get; init; }
    }
}
