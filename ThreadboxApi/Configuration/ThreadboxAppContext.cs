using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Configuration
{
    public class ThreadboxAppContext : IScopedService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public ThreadboxAppContext(IServiceProvider services)
        {
            _httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
            _userManager = services.GetRequiredService<UserManager<User>>();
        }

        public Guid UserId
        {
            get
            {
                var id = _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                return Guid.Parse(id);
            }
        }

        public async Task<User> GetUserAsync()
        {
            return await _userManager.FindByIdAsync(UserId.ToString());
        }

        public async Task<List<string>> GetRolesAsync()
        {
            var user = await GetUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    }
}