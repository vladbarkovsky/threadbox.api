using Serilog.Context;
using System.Diagnostics;
using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Web
{
    public class TraceIdLoggingMidleware : IMiddleware, ITransientService
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            using (LogContext.PushProperty("TraceId", traceId))
            {
                await next(context);
            }
        }
    }
}
