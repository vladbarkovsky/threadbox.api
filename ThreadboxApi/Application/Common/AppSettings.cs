namespace ThreadboxApi.Application.Common
{
    public class AppSettings
    {
        public const string DevDbConnectionString = "ThreadboxApiDev";

        public const string CorsPolicy = "Cors:Policy";
        public const string CorsOrigins = "Cors:Origins";
        public const string CorsMethods = "Cors:Methods";
        public const string CorsHeaders = "Cors:Headers";

        public const string JwtValidAudience = "Jwt:ValidAudience";
        public const string JwtValidIssuer = "Jwt:ValidIssuer";
        public const string JwtSecurityKey = "Jwt:SecurityKey";
        public const string JwtExpirationTimeSeconds = "Jwt:ExpirationTimeSeconds";

        public const string DefaultAdminUserName = "DefaultAdminCredentials:UserName";
        public const string DefaultAdminPassword = "DefaultAdminCredentials:Password";

        public const string AngularClientBaseUrl = "AngularClientBaseUrl";
        public const string RegistrationKeyExpirationTimeSeconds = "RegistrationKeyExpirationTimeSeconds";
    }
}