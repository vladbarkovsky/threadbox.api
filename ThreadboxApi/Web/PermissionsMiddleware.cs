using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Web
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
            if (context.Request.Path == "/api/Boards/GetBoardsList")
            {
                var x = context.User.Identity.AuthenticationType;
            }

            if (context.User.Identity.AuthenticationType == "Identity.Application" || context.User.Identity.AuthenticationType == null)
            {
                await _next(context);
                return;
            }

            var userId = context.User.FindFirst("sub")?.Value;

            //if (userId == null)
            //{
            //    await _next(context);
            //    return;
            //}

            var permissionsIdentity = await identityService.GetUserPermissionsIdentity(userId);

            if (permissionsIdentity != null)
            {
                context.User.AddIdentity(permissionsIdentity);
            }

            await _next(context);
        }
    }
}