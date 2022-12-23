namespace ThreadboxApi.Configuration
{
	public class AppSettings
	{
		public const string DbConnectionString = "ThreadboxApiDev";

		public const string CorsPolicy = "Cors:Policy";
		public const string CorsOrigins = "Cors:Origins";
		public const string CorsMethods = "Cors:Methods";

		public const string JwtSecurityKey = "Jwt:SecurityKey";
		public const string JwtValidAudience = "Jwt:ValidAudience";
		public const string JwtValidIssuer = "Jwt:ValidIssuer";
		public const string JwtExpirationTime = "Jwt:ExpirationTime";
	}
}