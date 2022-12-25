namespace ThreadboxApi.Configuration
{
	public class AppSettings
	{
		public const string DbConnectionString = "ThreadboxApiDev";

		public const string CorsPolicy = "Cors:Policy";
		public const string CorsOrigins = "Cors:Origins";
		public const string CorsMethods = "Cors:Methods";

		public const string JwtValidAudience = "Jwt:ValidAudience";
		public const string JwtValidIssuer = "Jwt:ValidIssuer";

		public const string JwtAuthenticationSecurityKey = "Jwt:Authentication:SecurityKey";
		public const string JwtAuthenticationExpirationTime = "Jwt:Authentication:ExpirationTime";

		public const string JwtRegistrationSecurityKey = "Jwt:Registration:SecurityKey";
		public const string JwtRegistrationExpirationTime = "Jwt:Registration:ExpirationTime";
	}
}