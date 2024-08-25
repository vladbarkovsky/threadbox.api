using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Configuration
{
    public class ErrorHandlingStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddExceptionHandler(options =>
            {
                options.AllowStatusCode404Response = true;

                options.ExceptionHandler = async context =>
                {
                    var problemDetailsService = context.RequestServices.GetRequiredService<ProblemDetailsService>();
                    var exception = context.Features.Get<IExceptionHandlerFeature>().Error;

                    ProblemDetails problemDetails;

                    if (exception is HttpResponseException httpResponseException)
                    {
                        context.Response.StatusCode = httpResponseException.StatusCode;
                        problemDetails = problemDetailsService.GetProblemDetails(httpResponseException.StatusCode);
                        problemDetails.Detail = httpResponseException.Message;
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        problemDetails = problemDetailsService.GetProblemDetails(StatusCodes.Status500InternalServerError);
                    }

                    context.Response.ContentType = "application/problem+json; charset=utf-8";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                };
            });

            services.AddSingleton<ProblemDetailsFactory, ApplicationProblemDetailsFactory>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler();
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}