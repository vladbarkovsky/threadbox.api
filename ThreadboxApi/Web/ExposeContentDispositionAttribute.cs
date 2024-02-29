using Microsoft.AspNetCore.Mvc.Filters;

namespace ThreadboxApi.Web
{
    /// <summary>
    /// Adds "Content-Disposition" to "Access-Control-Expose-Headers" header.
    /// </summary>
    public class ExposeContentDispositionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", new string[] { "Content-Disposition" });
        }
    }
}
