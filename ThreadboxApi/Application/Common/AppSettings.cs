namespace ThreadboxApi.Application.Common
{
    public class AppSettings
    {
        public class ConnectionStrings
        {
            public const string Development = "Development";
        }

        public const string ClientBaseUrl = "ClientBaseUrl";

        public class DefaultAdminCredentials
        {
            public const string UserName = "DefaultAdminCredentials:UserName";
            public const string Password = "DefaultAdminCredentials:Password";
        }

        public const string RegistrationKeyExpirationTimeSeconds = "RegistrationKeyExpirationTimeSeconds";
    }
}