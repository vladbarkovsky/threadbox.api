using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Dtos;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Services
{
	public class AuthenticationService : IScopedService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		private readonly UserManager<User> _userManager;
		private readonly JwtService _jwtService;

		public AuthenticationService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_mapper = services.GetRequiredService<IMapper>();
			_configuration = services.GetRequiredService<IConfiguration>();
			_userManager = services.GetRequiredService<UserManager<User>>();
			_jwtService = services.GetRequiredService<JwtService>();
		}

		public async Task<string> Login(LoginFormDto loginFormDto)
		{
			var user = await _userManager.FindByNameAsync(loginFormDto.UserName);

			if (user == null)
			{
				throw new HttpResponseException("Can't found user with such name.");
			}

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginFormDto.Password);

			if (!isPasswordCorrect)
			{
				throw new HttpResponseException("Password is incorrect.");
			}

			return CreateAuthenticationToken(user.Id);
		}

		public async Task Register(RegistrationFormDto registrationFormDto)
		{
			var isTokenAccepted = await UseRegistrationTokenAsync(registrationFormDto.RegistrationToken);

			if (!isTokenAccepted)
			{
				throw new HttpResponseException("Registration token was not accepted.");
			}

			var user = _mapper.Map<User>(registrationFormDto);
			var identityResult = await _userManager.CreateAsync(user, registrationFormDto.Password);

			if (!identityResult.Succeeded)
			{
				throw new HttpResponseException("Something went wrong during user creation.");
			}
		}

		public string CreateAuthenticationToken(Guid userId)
		{
			var jwtConfiguration = new JwtConfiguration
			{
				SecurityKey = AppSettings.JwtAuthenticationSecurityKey,
				ExpirationTimeS = AppSettings.JwtAuthenticationExpirationTimeS,
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
				ExpirationTimeS = AppSettings.JwtRegistrationExpirationTimeS,
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

		private async Task<bool> UseRegistrationTokenAsync(string token)
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
			var lifetime = TimeSpan.FromSeconds(Convert.ToInt32(_configuration[AppSettings.JwtRegistrationExpirationTimeS]!));
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