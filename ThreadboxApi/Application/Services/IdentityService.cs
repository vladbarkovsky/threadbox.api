using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Application.Services
{
    public class IdentityService : IScopedService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
    }
}