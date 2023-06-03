using System.IdentityModel.Tokens.Jwt;

namespace ThreadboxApi.Configuration
{
    public class JwtAccessTokenRefreshMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public JwtAccessTokenRefreshMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            ExecuteOperationsAsync(httpContext);
            await _requestDelegate(httpContext);
        }

        private void ExecuteOperationsAsync(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers.Authorization;

            if (!authorizationHeader.Any())
            {
                return;
            }

            var accessToken = authorizationHeader.FirstOrDefault();
            var handler = new JwtSecurityTokenHandler();
            // var ext = handler.ReadJwtToken(accessToken);
            System.Diagnostics.Debug.WriteLine(accessToken);
        }
    }
}