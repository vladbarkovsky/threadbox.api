using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Application.Services
{
    public class IdentityService : IScopedService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(UserManager<AppUser> userManager)
        { }

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
    }
}