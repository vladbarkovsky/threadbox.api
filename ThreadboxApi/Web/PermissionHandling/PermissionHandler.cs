using Microsoft.AspNetCore.Authorization;
using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.Web.PermissionHandling
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var hasPermission = context.User.HasClaim(PermissionContants.ClaimType, requirement.Permission);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}