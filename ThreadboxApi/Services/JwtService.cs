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
			var lifetime = Convert.ToInt32(_configuration[jwtConfiguration.ExpirationTime]!);

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

		/// <exception cref="SecurityTokenValidationException"></exception>
		public List<Claim> DecryptToken(JwtToDecrypt jwt)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[jwt.SecurityKey]!));

			var validationParams = new TokenValidationParameters
			{
				IssuerSigningKey = securityKey,
				ValidateIssuerSigningKey = true,

				ValidIssuer = _configuration[AppSettings.JwtValidIssuer],
				ValidateIssuer = true,

				ValidAudience = _configuration[AppSettings.JwtValidAudience],
				ValidateAudience = true,

				RequireExpirationTime = true,
				ClockSkew = TimeSpan.Zero,
				ValidateLifetime = true,
			};

			var handler = new JwtSecurityTokenHandler();
			var claims = handler.ValidateToken(jwt.Token, validationParams, out _).Claims;

			var hasRequiredClaims = claims
				.Select(x => x.Type)
				.Intersect(jwt.RequiredClaimTypes)
				.SequenceEqual(jwt.RequiredClaimTypes);

			if (!hasRequiredClaims)
			{
				throw new SecurityTokenValidationException("Required claims are missing.");
			}

			return claims.ToList();
		}
	}

	public class JwtConfiguration
	{
		public string SecurityKey { get; set; } = null!;
		public string ExpirationTime { get; set; } = null!;
		public List<Claim> Claims { get; set; } = null!;
	}

	public class JwtToDecrypt
	{
		public string Token { get; set; } = null!;
		public string SecurityKey { get; set; } = null!;
		public List<string> RequiredClaimTypes { get; set; } = null!;
	}
}