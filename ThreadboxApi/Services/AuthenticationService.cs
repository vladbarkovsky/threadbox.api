using Microsoft.EntityFrameworkCore;
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
		private readonly ThreadboxDbContext _dbContext;
		private readonly IConfiguration _configuration;

		public AuthenticationService(IServiceProvider services)
		{
			_jwtService = services.GetRequiredService<JwtService>();
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_configuration = services.GetRequiredService<IConfiguration>();
		}

		public string CreateAuthenticationToken(Guid userId)
		{
			var jwtConfiguration = new JwtConfiguration
			{
				SecurityKey = AppSettings.JwtAuthenticationSecurityKey,
				ExpirationTime = AppSettings.JwtAuthenticationExpirationTime,
				Claims = new List<Claim>
				{
					new Claim(Configuration.ClaimTypes.UserId, userId.ToString())
				}
			};

			return _jwtService.CreateToken(jwtConfiguration);
		}

		public async Task<string> CreateRegistrationTokenAsync()
		{
			await RemoveExpiredRegistrationKeysAsync();

			var registrationKeyValue = Guid.NewGuid();

			var jwtConfiguration = new JwtConfiguration
			{
				SecurityKey = AppSettings.JwtRegistrationSecurityKey,
				ExpirationTime = AppSettings.JwtRegistrationExpirationTime,
				Claims = new List<Claim>
				{
					new Claim(Configuration.ClaimTypes.RegistrationKey, registrationKeyValue.ToString())
				}
			};

			var token = _jwtService.CreateToken(jwtConfiguration);

			var registrationKey = new RegistrationKey
			{
				Value = registrationKeyValue,
				CreatedAt = DateTimeOffset.UtcNow
			};

			await _dbContext.RegistrationKeys.AddAsync(registrationKey);
			await _dbContext.SaveChangesAsync();

			return token;
		}

		public async Task<bool> UseRegistrationTokenAsync(string token)
		{
			string? tokenRegistrationKey = TryGetRegistrationKey(token);

			if (tokenRegistrationKey == null)
			{
				return false;
			}

			var dbRegistrationKey = await _dbContext.RegistrationKeys.FirstOrDefaultAsync(x => x.Value.ToString() == tokenRegistrationKey);

			if (dbRegistrationKey == null)
			{
				return false;
			}

			_dbContext.RegistrationKeys.Remove(dbRegistrationKey);
			await _dbContext.SaveChangesAsync();

			return true;
		}

		private async Task RemoveExpiredRegistrationKeysAsync()
		{
			var lifetime = TimeSpan.FromSeconds(Convert.ToInt32(_configuration[AppSettings.JwtRegistrationExpirationTime]!));
			var expiredKeys = _dbContext.RegistrationKeys.Where(x => x.CreatedAt - DateTimeOffset.UtcNow < lifetime);
			_dbContext.RemoveRange(expiredKeys);
			await _dbContext.SaveChangesAsync();
		}

		public string? TryGetRegistrationKey(string token)
		{
			var securityKey = Encoding.UTF8.GetBytes(_configuration[AppSettings.JwtRegistrationSecurityKey]!);

			var validationParams = new TokenValidationParameters
			{
				IssuerSigningKey = new SymmetricSecurityKey(securityKey),
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
			IEnumerable<Claim> claims;

			try
			{
				claims = handler.ValidateToken(token, validationParams, out _).Claims;
			}
			catch (Exception ex) when (ex is SecurityTokenValidationException || ex is ArgumentException)
			{
				return null;
			}

			var registrationKeyClaim = claims.FirstOrDefault(x => x.Type == Configuration.ClaimTypes.RegistrationKey);

			if (registrationKeyClaim == null)
			{
				return null;
			}

			return registrationKeyClaim.Value;
		}
	}
}