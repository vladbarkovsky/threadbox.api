using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
                    var exception = httpContext.Features.Get<IExceptionHandlerFeature>().Error;
                    ProblemDetails problemDetails;

                    if (exception is HttpResponseException httpResponseException)
                    {
                        httpContext.Response.StatusCode = httpResponseException.StatusCode;
                        problemDetails = ErrorHandlingExtensions.GetProblemDetails(httpResponseException.StatusCode);
                        problemDetails.Detail = httpResponseException.Message;
                    }
                    else
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        problemDetails = ErrorHandlingExtensions.GetProblemDetails(httpContext.Response.StatusCode);
                    }

                    problemDetails.Instance = httpContext.Request.Path + httpContext.Request.QueryString;
                    httpContext.Response.ContentType = "application/problem+json";
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                };
            });
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler();
            app.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}