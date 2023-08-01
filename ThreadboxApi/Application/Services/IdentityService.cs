using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Application.Services
{
    public class IdentityService : IScopedService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtService _jwtService;

        public IdentityService(UserManager<AppUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<string> SignIn(string userName, string password)
        {
            var appUser = await _userManager.FindByNameAsync(userName);
            HttpResponseException.ThrowNotFoundIfNull(appUser);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(appUser, password);

            if (!isPasswordCorrect)
            {
                throw new HttpResponseException("Password is incorrect.");
            }

            return _jwtService.CreateAccessToken(appUser.Id);
        }

        public async Task<AppUser> SignUp(string userName, string password)
        {
            var appUser = new AppUser
            {
                UserName = userName
            };

            // TODO: Handle result errors
            var result = await _userManager.CreateAsync(appUser, password);

            return await _userManager.FindByNameAsync(appUser.UserName);
        }
    }
}