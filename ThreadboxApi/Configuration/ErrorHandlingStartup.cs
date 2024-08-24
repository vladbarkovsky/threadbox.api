using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Encodings.Web;
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

                options.ExceptionHandler = async httpContext =>
                {
                    var problemDetailsService = httpContext.RequestServices.GetRequiredService<ProblemDetailsService>();
                    var exception = httpContext.Features.Get<IExceptionHandlerFeature>().Error;

                    ProblemDetails problemDetails;

                    if (exception is HttpResponseException httpResponseException)
                    {
                        httpContext.Response.StatusCode = httpResponseException.StatusCode;
                        problemDetails = problemDetailsService.GetProblemDetails(httpResponseException.StatusCode);
                        problemDetails.Detail = httpResponseException.Message;
                    }
                    else
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        problemDetails = problemDetailsService.GetProblemDetails(StatusCodes.Status500InternalServerError);
                    }

                    httpContext.Response.ContentType = "application/problem+json; charset=utf-8";
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
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