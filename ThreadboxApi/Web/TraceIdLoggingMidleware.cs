using Serilog.Context;
using System.Diagnostics;

namespace ThreadboxApi.Web
{
    public class TraceIdLoggingMidleware
    {
        private readonly RequestDelegate _next;

        public TraceIdLoggingMidleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

            if (traceId == null)
            {
                await _next(context);
                return;
            }

            using (LogContext.PushProperty("TraceId", traceId))
            {
                await _next(context);
            }
        }
    }
}