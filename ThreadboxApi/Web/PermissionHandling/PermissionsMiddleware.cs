using System.Security.Claims;
using ThreadboxApi.Application.Common.Helpers;
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

            // IdentityServer stores user ID in subject claim
            // JWT specification: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.2
            var userId = context.User.FindFirstValue("sub");
            HttpResponseException.ThrowNotFoundIfNull(userId);

            var permissionsIdentity = await identityService.GetPermissionsIdentity(userId);

            if (permissionsIdentity.Claims.Any())
            {
                context.User.AddIdentity(permissionsIdentity);
            }

            await _next(context);
        }
    }
}