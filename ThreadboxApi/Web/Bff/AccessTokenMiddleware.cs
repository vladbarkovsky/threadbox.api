using ThreadboxApi.Application.Bff;
using ThreadboxApi.ServiceLifetimes;

namespace ThreadboxApi.Web.Bff
{
    public class AccessTokenMiddleware : IMiddleware, IScoped
    {
        private readonly BffService _bffService;

        public AccessTokenMiddleware(BffService bffService)
        {
            _bffService = bffService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tokens = await _bffService.GetTokensAsync();

            if (tokens != null)
            {
                context.Request.Headers.Authorization = "Bearer " + tokens.AccessToken;
            }

            await next(context);
        }
    }
}
