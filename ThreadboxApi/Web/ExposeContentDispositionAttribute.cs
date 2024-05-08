using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace ThreadboxApi.Web
{
    /// <summary>
    /// Adds "Content-Disposition" to "Access-Control-Expose-Headers" header.
    /// </summary>
    public class ExposeContentDispositionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add(HeaderNames.AccessControlExposeHeaders, new string[] { HeaderNames.ContentDisposition });
        }
    }
}