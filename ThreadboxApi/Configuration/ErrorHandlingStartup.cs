using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Configuration
{
    public class ErrorHandlingStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddExceptionHandler(void (ExceptionHandlerOptions options) =>
            {
                options.AllowStatusCode404Response = true;

                options.ExceptionHandler = async Task (HttpContext context) =>
                {
                    ProblemDetailsService problemDetailsService = context.RequestServices.GetRequiredService<ProblemDetailsService>();
                    Exception exception = context.Features.Get<IExceptionHandlerFeature>().Error;
                    ProblemDetails problemDetails;

                    if (exception is HttpResponseException httpResponseException)
                    {
                        context.Response.StatusCode = httpResponseException.StatusCode;
                        problemDetails = problemDetailsService.GetProblemDetails(httpResponseException.StatusCode);
                        problemDetails.Detail = httpResponseException.Message;
                    }
                    else if (exception is DbUpdateConcurrencyException)
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        problemDetails = problemDetailsService.GetProblemDetails(StatusCodes.Status409Conflict);
                        problemDetails.Detail = "The record was changed by another request.";
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
