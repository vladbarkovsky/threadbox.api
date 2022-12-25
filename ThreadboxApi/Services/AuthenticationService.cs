using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
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
					new Claim(ClaimTypeConstants.UserId, userId.ToString())
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

		public async Task<bool> UseRegistrationTokenAsync(string token)
		{
			var jwt = new JwtToDecrypt
			{
				Token = token,
				SecurityKey = AppSettings.JwtRegistrationSecurityKey,
				RequiredClaimTypes = new List<string>
				{
					ClaimTypeConstants.RegistrationKey
				}
			};

			List<Claim> claims;

			try
			{
				claims = _jwtService.DecryptToken(jwt);
			}
			catch (Exception ex) when (ex is SecurityTokenValidationException || ex is ArgumentException)
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

		private async Task RemoveExpiredRegistrationKeysAsync()
		{
			var lifetime = TimeSpan.FromSeconds(Convert.ToInt32(_configuration[AppSettings.JwtRegistrationExpirationTime]!));
			var expiredKeys = _dbContext.RegistrationKeys.Where(x => x.CreatedAt - DateTimeOffset.UtcNow < lifetime);
			_dbContext.RemoveRange(expiredKeys);
			await _dbContext.SaveChangesAsync();
		}
	}
}