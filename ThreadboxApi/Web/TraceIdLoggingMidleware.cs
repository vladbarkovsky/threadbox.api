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
            using (LogContext.PushProperty("TraceId", Activity.Current.Id))
            {
                await _next(context);
            }
        }
    }
}