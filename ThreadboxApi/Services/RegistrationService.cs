using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class RegistrationService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly JwtService _jwtService;
		private readonly IConfiguration _configuration;

		public RegistrationService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_jwtService = services.GetRequiredService<JwtService>();
			_configuration = services.GetRequiredService<IConfiguration>();
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
					new Claim(ClaimTypeConstants.RegistrationKey, registrationKeyValue.ToString())
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

		private async Task RemoveExpiredRegistrationKeysAsync()
		{
			var lifetime = TimeSpan.FromSeconds(Convert.ToInt32(_configuration[AppSettings.JwtRegistrationExpirationTime]!));
			var expiredKeys = _dbContext.RegistrationKeys.Where(x => x.CreatedAt - DateTimeOffset.UtcNow < lifetime);
			_dbContext.RemoveRange(expiredKeys);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<bool> CheckRegistrationTokenAsync(string token)
		{
			var jwt = new JwtToDecrypt
			{
				Token = token,
				SecurityKey = AppSettings.JwtRegistrationSecurityKey,
				RequiredClaimTypes = new List<string> { ClaimTypeConstants.RegistrationKey }
			};

			List<Claim> claims;

			try
			{
				claims = _jwtService.DecryptToken(jwt);
			}
			catch (SecurityTokenValidationException)
			{
				return false;
			}

			var registrationKeyClaim = claims.First(x => x.Type == ClaimTypeConstants.RegistrationKey);
			var registrationKey = await _dbContext.RegistrationKeys.FirstOrDefaultAsync(x => x.Value.ToString() == registrationKeyClaim!.Value);

			if (registrationKey == null)
			{
				return false;
			}

			_dbContext.RegistrationKeys.Remove(registrationKey);
			await _dbContext.SaveChangesAsync();

			return true;
		}
	}
}