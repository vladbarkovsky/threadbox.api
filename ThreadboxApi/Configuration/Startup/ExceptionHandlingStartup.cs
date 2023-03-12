using Microsoft.AspNetCore.Diagnostics;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Configuration.Startup
{
	public class ExceptionHandlingStartup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddExceptionHandler(options =>
			{
				options.AllowStatusCode404Response = true;

				// NOTE: Must be declared through options.ExceptionHandler - otherwise it won't work
				options.ExceptionHandler = httpContext =>
				{
					var exception = httpContext.Features.Get<IExceptionHandlerFeature>().Error;

					if (exception is HttpResponseException)
					{
						var httpResponseException = exception as HttpResponseException;
						httpContext.Response.StatusCode = (int)httpResponseException.ResponseCode;
					}

					return Task.CompletedTask;
				};
			});
		}

		public static void Configure(IApplicationBuilder app)
		{
			app.UseExceptionHandler();
		}
	}
}