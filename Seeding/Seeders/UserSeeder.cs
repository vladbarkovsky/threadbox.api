using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Identity.Roles;
using ThreadboxApi.Configuration;
using ThreadboxApi.ORM.Entities;

namespace Seeding.Seeders
{
    public class UserSeeder
    {
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeeder(IOptionsSnapshot<AppSettings> appSettings, UserManager<ApplicationUser> userManager)
        {
            _appSettings = appSettings;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var admin = new ApplicationUser
            {
                UserName = _appSettings.Value.DefaultAdminCredentials.Username
            };

            await _userManager.CreateAsync(admin, _appSettings.Value.DefaultAdminCredentials.Password);
            await _userManager.AddToRoleAsync(admin, AdminRole.Name);

            var manager = new ApplicationUser
            {
                UserName = "manager"
            };

            await _userManager.CreateAsync(manager, _appSettings.Value.DefaultAdminCredentials.Password);
            await _userManager.AddToRoleAsync(manager, ManagerRole.Name);
        }
    }
}
