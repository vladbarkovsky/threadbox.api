namespace ThreadboxApi.Configuration
{
	public class AppSettings
	{
		public const string DbConnectionString = "ThreadboxApiDev";

		public const string CorsPolicy = "Cors:Policy";
		public const string CorsOrigins = "Cors:Origins";
		public const string CorsMethods = "Cors:Methods";
		public const string CorsHeaders = "Cors:Headers";

		public const string JwtValidAudience = "Jwt:ValidAudience";
		public const string JwtValidIssuer = "Jwt:ValidIssuer";
		public const string JwtSecurityKey = "Jwt:Authentication:SecurityKey";
		public const string JwtExpirationTimeS = "Jwt:Authentication:ExpirationTimeS";

		public const string DefaultAdminUserName = "DefaultAdminCredentials:UserName";
		public const string DefaultAdminPassword = "DefaultAdminCredentials:Password";

		public const string AngularClientBaseUrl = "AngularClientBaseUrl";
		public const string RegistrationKeyExpirationTimeS = "RegistrationKeyExpirationTimeS";
	}
}