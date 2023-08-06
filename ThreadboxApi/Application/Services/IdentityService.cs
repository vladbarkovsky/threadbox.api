using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Application.Services
{
    public class IdentityService : IScopedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public IdentityService(UserManager<ApplicationUser> userManager, JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<string> SignIn(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);
            HttpResponseException.ThrowNotFoundIfNull(user);

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordCorrect)
            {
                throw new HttpResponseException("Password is incorrect.");
            }

            return _jwtService.CreateAccessToken(user.Id);
        }

        public async Task<ApplicationUser> SignUp(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName
            };

            // TODO: Handle result errors
            var result = await _userManager.CreateAsync(user, password);

            return await _userManager.FindByNameAsync(user.UserName);
        }
    }
}