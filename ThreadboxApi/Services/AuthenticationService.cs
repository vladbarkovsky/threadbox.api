using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;

namespace ThreadboxApi.Services
{
	public class AuthenticationService : IScopedService
	{
		private readonly IConfiguration _configuration;

		public AuthenticationService(IServiceProvider services)
		{
			_configuration = services.GetRequiredService<IConfiguration>();
		}

		public string GenerateUserToken(string userId)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[AppSettings.JwtSecurityKey]!));
			var lifetime = int.Parse(_configuration[AppSettings.JwtExpirationTime]!);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Audience = _configuration[AppSettings.JwtValidAudience],
				Issuer = _configuration[AppSettings.JwtValidIssuer],
				Subject = new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimConstants.UserId, userId)
				}),
				Expires = DateTime.UtcNow.AddSeconds(lifetime),
				NotBefore = DateTime.UtcNow,
				SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}