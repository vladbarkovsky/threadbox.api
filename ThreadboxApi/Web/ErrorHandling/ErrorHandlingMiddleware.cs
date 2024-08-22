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

        public async Task InvokeAsync(HttpContext context, ProblemDetailsService problemDetailsService)
        {
            await _next(context);

            if (context.Response.HasStarted)
            {
                return;
            }

            if (context.Response.StatusCode > 399 && context.Response.StatusCode < 600)
            {
                var problemDetails = problemDetailsService.GetProblemDetails(context.Response.StatusCode);
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                return;
            }
        }
    }
}