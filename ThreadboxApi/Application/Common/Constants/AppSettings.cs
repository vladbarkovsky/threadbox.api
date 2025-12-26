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
        public string ClientBaseUrl { get; set; }

        public DefaultAdminCredentialsOptions DefaultAdminCredentials { get; set; }

        public class DefaultAdminCredentialsOptions
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public int AccessTokenLifetimeSeconds { get; set; }
        public int AbsoluteRefreshTokenLifetimeSeconds { get; set; }
        public int RegistrationKeyLifetimeSeconds { get; set; }
        public string ClientSecret { get; set; }
    }
}