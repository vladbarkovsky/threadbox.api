using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Identity.Models;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Services
{
    public class IdentityService : IScopedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _applicationContext;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationContext applicationContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationContext = applicationContext;
        }

        public async Task<UserSession> GetUserSessionAsync()
        {
            ApplicationUser user = await _userManager.FindByIdAsync(_applicationContext.CurrentUserId);
            List<Claim> permissionClaims = await GetPermissionClaimsAsync(user);

            return new UserSession
            {
                Username = user.UserName,
                Permissions = permissionClaims.Select(string (Claim claim) => claim.Value)
            };
        }

        public async Task<List<Claim>> GetPermissionClaimsAsync()
        {
            ApplicationUser user = await _userManager.FindByIdAsync(_applicationContext.CurrentUserId);
            return await GetPermissionClaimsAsync(user);
        }

        private async Task<List<Claim>> GetPermissionClaimsAsync(ApplicationUser user)
        {
            var permissionClaims = new List<Claim>();
            IList<string> roles = await _userManager.GetRolesAsync(user);

            if (!roles.Any())
            {
                return permissionClaims;
            }

            IdentityRole role = await _roleManager.FindByNameAsync(roles.First());
            IList<Claim> claims = await _roleManager.GetClaimsAsync(role);

            permissionClaims.AddRange(claims.Where(bool (Claim claim) => claim.Type == PermissionConstants.ClaimType));
            return permissionClaims;
        }
    }
}
