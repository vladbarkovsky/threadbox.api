using System.Text.Json;
using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Web.ErrorHandling
{
    public class ErrorHandlingMiddleware : IMiddleware, ITransientService
    {
        private readonly ProblemDetailsService _problemDetailsService;

        public ErrorHandlingMiddleware(ProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next(context);

            if (context.Response.HasStarted)
            {
                return;
            }

            if (context.Response.StatusCode > 399 && context.Response.StatusCode < 600)
            {
                var problemDetails = _problemDetailsService.GetProblemDetails(context.Response.StatusCode);
                context.Response.ContentType = "application/problem+json; charset=utf-8";
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                return;
            }
        }
    }
}