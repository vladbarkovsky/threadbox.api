using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Dtos;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Infrastructure.Identity
{
    public class IdentityService : IScopedService
    {
        private readonly Persistence.ThreadboxDbContext _dbContext;
        private readonly ThreadboxAppContext _appContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly JwtService _jwtService;

        private TimeSpan RegistrationKeyLifetime
        {
            get
            {
                return TimeSpan.FromSeconds(Convert.ToInt32(_configuration[AppSettings.RegistrationKeyExpirationTimeSeconds]));
            }
        }

        public IdentityService(IServiceProvider services)
        {
            _dbContext = services.GetRequiredService<Persistence.ThreadboxDbContext>();
            _appContext = services.GetRequiredService<ThreadboxAppContext>();
            _mapper = services.GetRequiredService<IMapper>();
            _configuration = services.GetRequiredService<IConfiguration>();
            _userManager = services.GetRequiredService<UserManager<User>>();
            _jwtService = services.GetRequiredService<JwtService>();
        }

        public async Task<string> Login(LoginFormDto loginFormDto)
        {
            var user = await _userManager.FindByNameAsync(loginFormDto.UserName);
            HttpResponseException.ThrowNotFoundIfNull(user);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginFormDto.Password);

            if (!isPasswordCorrect)
            {
                throw new HttpResponseException("Password is incorrect.");
            }

            return _jwtService.CreateAccessToken(user.Id);
        }

        public string RefreshAccessToken()
        {
            return _jwtService.CreateAccessToken(_appContext.UserId);
        }

        public async Task<string> CreateRegistrationUrlAsync()
        {
            RemoveExpiredRegistrationKeys();

            var registrationKey = new RegistrationKey
            {
                CreatedAt = DateTimeOffset.UtcNow,
            };

            var entityEntry = _dbContext.RegistrationKeys.Add(registrationKey);
            await _dbContext.SaveChangesAsync();

            return string.Format(
                Constants.RegistrationUrl,
                _configuration[AppSettings.AngularClientBaseUrl],
                entityEntry.Entity.Id);
        }

        public async Task ValidateRegistrationKeyAsync(Guid registrationKeyId)
        {
            RemoveExpiredRegistrationKeys();
            await _dbContext.SaveChangesAsync();

            var registrationKey = await _dbContext.RegistrationKeys
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == registrationKeyId);

            HttpResponseException.ThrowNotFoundIfNull(registrationKey);
        }

        public async Task Register(RegistrationFormDto registrationFormDto)
        {
            RemoveExpiredRegistrationKeys();

            var registrationKey = await _dbContext.RegistrationKeys
                .FirstOrDefaultAsync(x => x.Id == registrationFormDto.RegistrationKeyId);

            HttpResponseException.ThrowNotFoundIfNull(registrationKey);

            _dbContext.RegistrationKeys.Remove(registrationKey);
            var user = _mapper.Map<User>(registrationFormDto);
            await _userManager.CreateAsync(user, registrationFormDto.Password);
        }

        private void RemoveExpiredRegistrationKeys()
        {
            var lifetime = RegistrationKeyLifetime;
            var now = DateTimeOffset.UtcNow;
            var expiredKeys = _dbContext.RegistrationKeys.Where(x => x.CreatedAt + lifetime < now);
            _dbContext.RemoveRange(expiredKeys);
        }
    }
}