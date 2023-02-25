﻿using Microsoft.AspNetCore.Diagnostics;
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

				options.ExceptionHandler = httpContext =>
				{
					var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

					if (exception is HttpResponseException)
					{
						httpContext.Response.StatusCode = (exception as HttpResponseException)!.HttpErrorResponseCode;
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