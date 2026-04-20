namespace ThreadboxApi.Application.Common.Constants
{
    public class AppSettings
    {
        public ConnectionStringsOptions ConnectionStrings { get; set; }

        public class ConnectionStringsOptions
        {
            public string Database { get; set; }
        }

        public string BaseUrl { get; set; }
        public string FrontendBaseUrl { get; set; }

        public DefaultAdminCredentialsOptions DefaultAdminCredentials { get; set; }

        public class DefaultAdminCredentialsOptions
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public int AbsoluteRefreshTokenLifetimeSeconds { get; set; }
        public string OidcBffClientSecret { get; set; }
        public string SslPassword { get; set; }
    }
}