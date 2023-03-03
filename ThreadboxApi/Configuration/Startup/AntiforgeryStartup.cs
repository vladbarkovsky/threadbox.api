using Microsoft.AspNetCore.Antiforgery;

namespace ThreadboxApi.Configuration.Startup
{
	public static class AntiForgeryStartup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddAntiforgery(options => options.HeaderName = "XSRF-TOKEN");
		}

		public static void Configure(IApplicationBuilder app)
		{
			app.UseAntiforgeryToken();
		}
	}

	public class AntiForgeryTokenMiddleware
	{
		private readonly RequestDelegate _requestDelegate;
		private readonly IAntiforgery _antiforgery;

		public AntiForgeryTokenMiddleware(RequestDelegate requestDelegate, IAntiforgery antiforgery)
		{
			_requestDelegate = requestDelegate;
			_antiforgery = antiforgery;
		}

		public Task Invoke(HttpContext httpContext)
		{
			if (httpContext.Request.Headers.ContainsKey("XSRF-TOKEN"))
			{
				_antiforgery.ValidateRequestAsync(httpContext);
			}

			var tokens = _antiforgery.GetAndStoreTokens(httpContext);
			httpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
			{
				HttpOnly = false,
				Secure = true
			});

			return _requestDelegate(httpContext);
		}
	}

	public static class AntiforgeryTokenMiddlewareExtensions
	{
		public static IApplicationBuilder UseAntiforgeryToken(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<AntiForgeryTokenMiddleware>();
		}
	}
}