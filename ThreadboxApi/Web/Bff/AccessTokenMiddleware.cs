using System.Net.Http.Headers;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Web.Bff
{
    public class AccessTokenMiddleware : IMiddleware, ITransientService
    {
        private readonly BffTokensService _bffService;

        public AccessTokenMiddleware(BffTokensService bffService)
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
