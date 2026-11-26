namespace ThreadboxApi.Web.Language
{
    public class IdentityPageCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityPageCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.GetEndpoint() != null || !context.Request.Path.StartsWithSegments("/Identity"))
            {
                await _next(context);
                return;
            }

            string language = RequestLanguage.Get(context.Request);
            string localizedPath = $"/{language}{context.Request.Path}{context.Request.QueryString}";
            context.Response.Redirect(localizedPath);
        }
    }
}
