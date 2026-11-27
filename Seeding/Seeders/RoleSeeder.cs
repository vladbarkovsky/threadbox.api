using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi;
using ThreadboxApi.Application.Identity.Permissions;

namespace Seeding.Seeders
{
    public class RoleSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleSeeder(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            IEnumerable<Type> roleTypes = Reflection.GetRoleTypes();

            foreach (Type roleType in roleTypes)
            {
                string roleName = Reflection.GetRoleName(roleType);
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                IdentityRole role = await _roleManager.FindByNameAsync(roleName);

                IEnumerable<string> rolePermissions = Reflection.GetRolePermissions(roleType);

                foreach (string permission in rolePermissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.ClaimType, permission));
                }
            }
        }
    }
}
