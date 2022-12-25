using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class AuthenticationService : IScopedService
	{
		private readonly JwtService _jwtService;

		public AuthenticationService(IServiceProvider services)
		{
			_jwtService = services.GetRequiredService<JwtService>();
		}

		public string CreateAuthenticationToken(Guid userId)
		{
			var jwtConfiguration = new JwtConfiguration
			{
				SecurityKey = AppSettings.JwtAuthenticationSecurityKey,
				ExpirationTime = AppSettings.JwtAuthenticationExpirationTime,
				Claims = new List<Claim>
				{
					new Claim(Configuration.ClaimTypeConstants.UserId, userId.ToString())
				}
			};

			return _jwtService.CreateToken(jwtConfiguration);
		}
	}
}