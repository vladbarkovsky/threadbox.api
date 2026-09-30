using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.ORM.Services
{
    public class RoleService : ITransientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        private List<IdentityRole> ExistingRoles { get; set; }

        public RoleService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public async Task SynchronizeRolesAsync()
        {
            IEnumerable<Type> roleTypes = Reflection.GetRoleTypes();
            IEnumerable<string> roleTypeNames = roleTypes.Select(string (Type type) => Reflection.GetRoleName(type));

            ExistingRoles = await _roleManager.Roles.ToListAsync();
            IEnumerable<string> existingRoleNames = ExistingRoles.Select(string (IdentityRole role) => role.Name);

            IEnumerable<string> roleToDeleteNames = existingRoleNames.Except(roleTypeNames);
            IEnumerable<string> roleToAddNames = roleTypeNames.Except(existingRoleNames);
            IEnumerable<string> roleToUpdateNames = roleTypeNames.Intersect(existingRoleNames);

            await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await DeleteRolesAsync(roleToDeleteNames);
                await AddRolesAsync(roleToAddNames);
                await UpdateRolesAsync(roleToUpdateNames);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task DeleteRolesAsync(IEnumerable<string> roleToDeleteNames)
        {
            foreach (string roleName in roleToDeleteNames)
            {
                IdentityRole roleToDelete = await _roleManager.FindByNameAsync(roleName);

                List<IdentityRoleClaim<string>> permissionClaims = await _dbContext.RoleClaims
                    .Where(bool (IdentityRoleClaim<string> claim) => claim.RoleId == roleToDelete.Id && claim.ClaimType == PermissionConstants.ClaimType)
                    .ToListAsync();

                _dbContext.RoleClaims.RemoveRange(permissionClaims);
                await _dbContext.SaveChangesAsync();

                await _roleManager.DeleteAsync(roleToDelete);
            }
        }

        private async Task AddRolesAsync(IEnumerable<string> roleToAddNames)
        {
            foreach (var roleName in roleToAddNames)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                IdentityRole role = await _roleManager.FindByNameAsync(roleName);

                HashSet<string> permissions = Reflection.GetRolePermissions(roleName);

                foreach (string permission in permissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.ClaimType, permission));
                }
            }
        }

        private async Task UpdateRolesAsync(IEnumerable<string> roleToUpdateNames)
        {
            foreach (string roleName in roleToUpdateNames)
            {
                HashSet<string> roleTypePermissions = Reflection.GetRolePermissions(roleName);

                IdentityRole role = ExistingRoles.Single(bool (IdentityRole role) => role.Name == roleName);

                List<IdentityRoleClaim<string>> existingPermissions = await _dbContext.RoleClaims
                    .Where(bool (IdentityRoleClaim<string> claim) => claim.RoleId == role.Id && claim.ClaimType == PermissionConstants.ClaimType)
                    .ToListAsync();

                IEnumerable<string> existingPermissionNames = existingPermissions.Select(string (IdentityRoleClaim<string> claim) => claim.ClaimValue);

                IEnumerable<IdentityRoleClaim<string>> permissionsToDelete = existingPermissions.ExceptBy(roleTypePermissions, x => x.ClaimValue);
                IEnumerable<string> permissionsToAdd = roleTypePermissions.Except(existingPermissionNames);

                _dbContext.RoleClaims.RemoveRange(permissionsToDelete);
                await _dbContext.SaveChangesAsync();

                foreach (string permission in permissionsToAdd)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.ClaimType, permission));
                }
            }
        }
    }
}