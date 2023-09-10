using Microsoft.AspNetCore.Authorization;
using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.Web
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        public PermissionHandler()
        { }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var hasPermission = context.User.HasClaim(PermissionContants.ClaimType, requirement.Permission);

            if (!hasPermission)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}