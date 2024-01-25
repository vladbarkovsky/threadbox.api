using System.Security.Claims;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Web.PermissionHandling
{
    public class PermissionsMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IdentityService identityService)
        {
            // We need to add permission claims only for HTTP requests with JWT authorization.
            // HTTP requests with JWT authorization result in the creation of Identity with authentication type "AuthenticationTypes.Federation"
            if (context.User.Identity.AuthenticationType != "AuthenticationTypes.Federation")
            {
                await _next(context);
                return;
            }

            var permissionClaims = await identityService.GetPermissionClaimsAsync();

            if (permissionClaims.Any())
            {
                var permissionsIdentity = new ClaimsIdentity(
                    permissionClaims,
                    nameof(PermissionsMiddleware),
                    "name",
                    "role");

                context.User.AddIdentity(permissionsIdentity);
            }

            await _next(context);
        }
    }
}