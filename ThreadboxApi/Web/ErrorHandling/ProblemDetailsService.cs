using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Web.ErrorHandling
{
    public class ProblemDetailsService : ISingletonService
    {
        private readonly ApiBehaviorOptions _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProblemDetailsService(IOptions<ApiBehaviorOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _httpContextAccessor = httpContextAccessor;
        }

        public ProblemDetails GetProblemDetails(int statusCode)
        {
            _options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData);

            var problemDetails = new ProblemDetails
            {
                Type = clientErrorData?.Link ?? "about:blank",
                Title = clientErrorData?.Title ?? "Something went wrong.",
                Status = statusCode,
                Instance = _httpContextAccessor.HttpContext.Request.Path + _httpContextAccessor.HttpContext.Request.QueryString
            };

            var traceId = Activity.Current?.Id ?? _httpContextAccessor.HttpContext.TraceIdentifier;

            if (traceId != null)
            {
                problemDetails.Extensions.Add("traceId", traceId);
            }

            return problemDetails;
        }

        public ValidationProblemDetails GetValidationProblemDetails(int statusCode, ModelStateDictionary modelStateDictionary)
        {
            var problemDetails = GetProblemDetails(statusCode);

            var validationProblemDetails = new ValidationProblemDetails(modelStateDictionary)
            {
                Type = problemDetails.Type,
                Title = "One or more validation errors occurred.",
                Status = problemDetails.Status,
                Instance = problemDetails.Instance,
            };

            foreach (var extension in problemDetails.Extensions)
            {
                validationProblemDetails.Extensions.Add(extension);
            }

            return validationProblemDetails;
        }
    }
}