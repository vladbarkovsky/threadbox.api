namespace ThreadboxApi.Configuration
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public string BaseUrl { get; set; }
        public string FrontendBaseUrl { get; set; }
        public string InformationCookieDomain { get; set; }
        public DefaultAdminCredentials DefaultAdminCredentials { get; set; }
        public int AccessTokenLifetimeSeconds { get; set; }
        public int AbsoluteRefreshTokenLifetimeSeconds { get; set; }
        public string OidcBffClientSecret { get; set; }
        public string SigningPfxPassword { get; set; }
        public string LuckyPennyLicenseKey { get; set; }
        public ApiSpecificationCredentials ApiSpecificationCredentials { get; set; }

    }

    public class ConnectionStrings
    {
        public string Postgres { get; set; }
    }

    public class DefaultAdminCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ApiSpecificationCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}