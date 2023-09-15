using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Infrastructure.Identity;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Application.Services
{
    public class IdentityService : IScopedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ClaimsIdentity> GetPermissionsIdentity(string userId)
        {
            var permissionsIdentity = new ClaimsIdentity(nameof(PermissionsMiddleware), "name", "role");

            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                return permissionsIdentity;
            }

            var role = await _roleManager.FindByNameAsync(roles.First());
            var claims = await _roleManager.GetClaimsAsync(role);

            if (!claims.Any())
            {
                return permissionsIdentity;
            }

            permissionsIdentity.AddClaims(claims);
            return permissionsIdentity;
        }
    }
}