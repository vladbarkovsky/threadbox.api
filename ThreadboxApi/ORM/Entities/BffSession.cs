namespace ThreadboxApi.ORM.Entities
{
    public class BffSession : BaseEntity
    {
        public string AccessToken { get; set; }
        public DateTimeOffset AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset SessionExpiresAt { get; set; }
    }
}
