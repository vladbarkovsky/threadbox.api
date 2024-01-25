namespace ThreadboxApi.Application.Common.Constants
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
            private const string Prefix = "DefaultAdminCredentials";
            public const string UserName = $"{Prefix}:UserName";
            public const string Password = $"{Prefix}:Password";
        }

        public const string RegistrationKeyExpirationTimeSeconds = "RegistrationKeyExpirationTimeSeconds";
    }
}