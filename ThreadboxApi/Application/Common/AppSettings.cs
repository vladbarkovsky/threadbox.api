namespace ThreadboxApi.Application.Common
{
    public class AppSettings
    {
        public const string RegistrationKeyExpirationTimeSeconds = "RegistrationKeyExpirationTimeSeconds";

        public class ConnectionStrings
        {
            public const string Dev = "ThreadboxApiDev";
        }

        public class Cors
        {
            public const string Policy = "Cors:Policy";
            public const string Origins = "Cors:Origins";
            public const string Methods = "Cors:Methods";
            public const string Headers = "Cors:Headers";
        }

        public class Jwt
        {
            public const string ValidAudience = "Jwt:ValidAudience";
            public const string ValidIssuer = "Jwt:ValidIssuer";
            public const string SecurityKey = "Jwt:SecurityKey";
            public const string ExpirationTimeSeconds = "Jwt:ExpirationTimeSeconds";
        }

        public class DefaultAdminCredentials
        {
            public const string UserName = "DefaultAdminCredentials:UserName";
            public const string Password = "DefaultAdminCredentials:Password";
        }
    }
}