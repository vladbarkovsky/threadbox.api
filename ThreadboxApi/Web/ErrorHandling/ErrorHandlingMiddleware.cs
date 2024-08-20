using System.Text.Json;

namespace ThreadboxApi.Web.ErrorHandling
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.HasStarted)
            {
                return;
            }

            if (context.Response.StatusCode > 399 && context.Response.StatusCode < 600)
            {
                var problemDetails = ErrorHandlingExtensions.GetProblemDetails(context.Response.StatusCode);
                problemDetails.Instance = context.Request.Path + context.Request.QueryString;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                return;
            }
        }
    }
}