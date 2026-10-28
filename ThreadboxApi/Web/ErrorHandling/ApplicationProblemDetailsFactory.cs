/// Copied from <see cref="DefaultProblemDetailsFactory"/>.
/// See https://github.com/dotnet/aspnetcore/blob/v6.0.0/src/Mvc/Mvc.Core/src/Infrastructure/DefaultProblemDetailsFactory.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace ThreadboxApi.Web.ErrorHandling
{
    public class ApplicationProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ProblemDetailsService _problemDetailsService;

        public ApplicationProblemDetailsFactory(IOptions<ApiBehaviorOptions> options, ProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            statusCode ??= StatusCodes.Status500InternalServerError;
            ProblemDetails problemDetails = _problemDetailsService.GetProblemDetails(statusCode.Value);
            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string title = null,
            string type = null,
            string detail = null,
            string instance = null)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= StatusCodes.Status400BadRequest;
            ValidationProblemDetails problemDetails = _problemDetailsService.GetValidationProblemDetails(statusCode.Value, modelStateDictionary);

            if (title != null)
            {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            return problemDetails;
        }
    }
}