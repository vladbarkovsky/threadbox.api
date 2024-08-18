using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.ORM.Services
{
    public class RoleSynchronizer : ITransientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        private List<IdentityRole> ExistingRoles { get; set; }

        public RoleSynchronizer(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task SyncAsync()
        {
            var roleTypes = Reflection.GetRoleTypes();
            var roleTypeNames = roleTypes.Select(x => Reflection.GetRoleName(x));

            ExistingRoles = await _roleManager.Roles.ToListAsync();
            var existingRoleNames = ExistingRoles.Select(x => x.Name);

            var roleToDeleteNames = existingRoleNames.Except(roleTypeNames);
            var roleToAddNames = roleTypeNames.Except(existingRoleNames);
            var roleToUpdateNames = roleTypeNames.Intersect(existingRoleNames);

            await DeleteRolesAsync(roleToDeleteNames);
            await AddRolesAsync(roleToAddNames);
            await UpdateRolesAsync(roleToUpdateNames);
        }

        private async Task DeleteRolesAsync(IEnumerable<string> roleToDeleteNames)
        {
            foreach (var roleName in roleToDeleteNames)
            {
                var roleToDelete = await _roleManager.FindByNameAsync(roleName);

                var permissionClaims = await _dbContext.RoleClaims
                    .Where(x => x.RoleId == roleToDelete.Id && x.ClaimType == PermissionConstants.ClaimType)
                    .ToListAsync();

                _dbContext.RoleClaims.RemoveRange(permissionClaims);
                await _roleManager.DeleteAsync(roleToDelete);
            }
        }

        private async Task AddRolesAsync(IEnumerable<string> roleToAddNames)
        {
            foreach (var roleName in roleToAddNames)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                var role = await _roleManager.FindByNameAsync(roleName);

                var permissions = Reflection.GetRolePermissions(roleName);

                foreach (var permission in permissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.ClaimType, permission));
                }
            }
        }

        private async Task UpdateRolesAsync(IEnumerable<string> roleToUpdateNames)
        {
            foreach (var roleName in roleToUpdateNames)
            {
                var roleTypePermissions = Reflection.GetRolePermissions(roleName);

                var role = ExistingRoles
                    .Where(x => x.Name == roleName)
                    .Single();

                var existingPermissions = await _dbContext.RoleClaims
                    .Where(x => x.RoleId == role.Id && x.ClaimType == PermissionConstants.ClaimType)
                    .ToListAsync();

                var existingPermissionNames = existingPermissions.Select(x => x.ClaimValue);

                var permissionsToDelete = existingPermissions.ExceptBy(roleTypePermissions, x => x.ClaimValue);
                var permissionsToAdd = roleTypePermissions.Except(existingPermissionNames);

                _dbContext.RoleClaims.RemoveRange(permissionsToDelete);

                foreach (var permission in permissionsToAdd)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.ClaimType, permission));
                }
            }
        }
    }
}