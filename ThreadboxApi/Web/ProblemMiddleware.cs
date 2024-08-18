using System.Net.Mime;

namespace ThreadboxApi.Web
{
    public class ProblemMiddleware
    {
        private readonly RequestDelegate _next;

        public ProblemMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.HasStarted)
                {
                    return;
                }

                if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 600)
                {
                    await HandleStatusCodeAsync(context);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.Response.WriteAsync(ex.Message);
        }

        private async Task HandleStatusCodeAsync(HttpContext context)
        {
            context.Response.ContentType = MediaTypeNames.Text.Plain;
            await context.Response.WriteAsync(context.Response.StatusCode.ToString());
        }
    }
}