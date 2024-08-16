using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Services
{
    public class IdentityService : IScopedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _appContext;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationContext appContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appContext = appContext;
        }

        /// <returns>authorized user permissions</returns>
        public async Task<List<string>> GetPermissionsAsync()
        {
            var permissionClaims = await GetPermissionClaimsAsync();
            return permissionClaims.Select(x => x.Value).ToList();
        }

        /// <returns>authorized user permission claims</returns>
        public async Task<List<Claim>> GetPermissionClaimsAsync()
        {
            var permissionClaims = new List<Claim>();

            var user = await _userManager.FindByIdAsync(_appContext.UserId);
            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                return permissionClaims;
            }

            var role = await _roleManager.FindByNameAsync(roles.First());
            var claims = await _roleManager.GetClaimsAsync(role);

            permissionClaims.AddRange(claims.Where(x => x.Type == PermissionConstants.ClaimType));
            return permissionClaims;
        }
    }
}