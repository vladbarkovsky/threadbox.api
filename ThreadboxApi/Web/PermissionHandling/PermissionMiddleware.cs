using System.Security.Claims;
using ThreadboxApi.Application.Identity;
using ThreadboxApi.Configuration.Identity;

namespace ThreadboxApi.Web.PermissionHandling
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IdentityService identityService)
        {
            if (context.User.Identity.AuthenticationType != AuthenticationSchemeConstants.AccessToken)
            {
                await _next(context);
                return;
            }

            var permissionClaims = await identityService.GetPermissionClaimsAsync();

            if (permissionClaims.Any())
            {
                var permissionsIdentity = new ClaimsIdentity(
                    claims: permissionClaims,
                    authenticationType: nameof(PermissionMiddleware),
                    nameType: "name",
                    roleType: "role");

                context.User.AddIdentity(permissionsIdentity);
            }

            await _next(context);
        }
    }
}
