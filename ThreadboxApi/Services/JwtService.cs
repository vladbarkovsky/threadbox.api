using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;

namespace ThreadboxApi.Services
{
	public class JwtService : IScopedService
	{
		private readonly IConfiguration _configuration;

		public JwtService(IServiceProvider services)
		{
			_configuration = services.GetRequiredService<IConfiguration>();
		}

		public string CreateToken(JwtConfiguration jwtConfiguration)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[jwtConfiguration.SecurityKey]!));
			var lifetime = Convert.ToInt32(_configuration[jwtConfiguration.ExpirationTimeS]!);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Audience = _configuration[AppSettings.JwtValidAudience],
				Issuer = _configuration[AppSettings.JwtValidIssuer],
				Subject = new ClaimsIdentity(jwtConfiguration.Claims),
				Expires = DateTime.UtcNow.AddSeconds(lifetime),
				NotBefore = DateTime.UtcNow,
				SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}

	public class JwtConfiguration
	{
		public string SecurityKey { get; set; } = null!;
		public string ExpirationTimeS { get; set; } = null!;
		public List<Claim> Claims { get; set; } = null!;
	}
}