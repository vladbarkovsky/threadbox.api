using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Immutable;
using System.Diagnostics;

namespace ThreadboxApi.Web.ErrorHandling
{
    public static class ErrorHandlingExtensions
    {
        private static IReadOnlyDictionary<int, ProblemDetails> ProblemDetailsMap { get; }

        static ErrorHandlingExtensions()
        {
            ProblemDetailsMap = new Dictionary<int, ProblemDetails>
            {
                {
                    StatusCodes.Status400BadRequest,
                    new()
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Title = "Bad Request",
                        Detail = "The request cannot be processed."
                    }
                },
                {
                    StatusCodes.Status401Unauthorized,
                    new()
                    {
                        Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                        Title = "Unauthorized",
                        Detail = "Authentication failed."
                    }
                },
                {
                    StatusCodes.Status403Forbidden,
                    new()
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                        Title = "Forbidden",
                        Detail = "Missing required permissions."
                    }
                },
                {
                    StatusCodes.Status404NotFound,
                    new()
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                        Title = "Not Found",
                        Detail = "The specified resource was not found."
                    }
                },
                {
                    StatusCodes.Status500InternalServerError,
                    new()
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                        Title = "Internal Server Error",
                        Detail = "An error occurred while processing request."
                    }
                }
            };
        }

        public static ProblemDetails GetProblemDetails(int statusCode)
        {
            if (statusCode < 400 || statusCode > 599)
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), "Problem details can only be obtained for the status code range 400-599.");
            }

            var problemDetailsFound = ProblemDetailsMap.TryGetValue(statusCode, out var problemDetails);

            if (!problemDetailsFound)
            {
                problemDetails = new ProblemDetails
                {
                    Type = "about:blank",
                    Title = "Unhandled Problem",
                    Detail = "Our API does not support RFC 7080 for this issue. Please contact tech support and we will fix it."
                };
            }

            problemDetails = new ProblemDetails
            {
                Type = problemDetails.Type,
                Title = problemDetails.Title,
                Detail = problemDetails.Detail,
            };

            problemDetails.Status = statusCode;
            problemDetails.Extensions.Add("traceId", Activity.Current.Id);

            return problemDetails;
        }
    }
}