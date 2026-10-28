using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace ThreadboxApi.Web
{
    /// <summary>
    /// Appends "Content-Disposition" to "Access-Control-Expose-Headers" header.
    /// </summary>
    internal sealed class ExposeContentDispositionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Append(HeaderNames.AccessControlExposeHeaders, HeaderNames.ContentDisposition);
        }
    }
}
