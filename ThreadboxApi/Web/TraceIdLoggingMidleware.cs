using Serilog.Context;
using System.Diagnostics;
using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Web
{
    public class TraceIdLoggingMidleware : IMiddleware, ITransientService
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            if (traceId == null)
            {
                await next(context);
                return;
            }

            using (LogContext.PushProperty("TraceId", traceId))
            {
                await next(context);
            }
        }
    }
}