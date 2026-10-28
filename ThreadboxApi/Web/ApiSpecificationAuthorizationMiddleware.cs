using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Web
{
    public class ApiSpecificationAuthorizationMiddleware
    {
        private IReadOnlySet<string> ProtectedRequestPaths { get; }

        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<AppSettings> _appSettings;

        public ApiSpecificationAuthorizationMiddleware(RequestDelegate next, IOptionsMonitor<AppSettings> appSettings)
        {
            ProtectedRequestPaths = new HashSet<string>
            {
                "/api/index.html",
                "/api/specification.json"
            };

            _next = next;
            _appSettings = appSettings;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            PathString requestPath = context.Request.Path;

            if (!ProtectedRequestPaths.Contains(requestPath))
            {
                await _next(context);
                return;
            }

            if (IsAuthorized(context.Request))
            {
                
                await _next(context);
                return;
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Headers.WWWAuthenticate = "Basic realm=\"Threadbox API specification\"";
        }

        private bool IsAuthorized(HttpRequest request)
        {
            if (!AuthenticationHeaderValue.TryParse(request.Headers.Authorization, out AuthenticationHeaderValue authenticationHeader)
                || !string.Equals(authenticationHeader.Scheme, "Basic", StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrWhiteSpace(authenticationHeader.Parameter))
            {
                return false;
            }

            string credentials;

            try
            {
                credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationHeader.Parameter));
            }
            catch (FormatException)
            {
                return false;
            }

            int separatorIndex = credentials.IndexOf(':');

            if (separatorIndex < 0)
            {
                return false;
            }

            string userName = credentials.Substring(0, separatorIndex);
            string password = credentials.Substring(separatorIndex + 1);

            return FixedTimeEquals(userName, _appSettings.CurrentValue.ApiSpecificationCredentials.Username)
                && FixedTimeEquals(password, _appSettings.CurrentValue.ApiSpecificationCredentials.Password);
        }

        private static bool FixedTimeEquals(string actualValue, string expectedValue)
        {
            byte[] actualBytes = Encoding.UTF8.GetBytes(actualValue);
            byte[] expectedBytes = Encoding.UTF8.GetBytes(expectedValue);

            return actualBytes.Length == expectedBytes.Length
                && CryptographicOperations.FixedTimeEquals(actualBytes, expectedBytes);
        }
    }
}
