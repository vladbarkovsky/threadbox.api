using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Encodings.Web;
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

            var validationProblemDetails = new ValidationProblemDetails(EscapeModelStateDictionary(modelStateDictionary))
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

        /// Copied from <see cref="ValidationProblemDetails"/> line 35.
        /// Why it's necessary is written here: https://josef.codes/beware-of-potential-xss-injections-when-using-problemdetails-in-asp-net-core/
        private static IDictionary<string, string[]> EscapeModelStateDictionary(ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            var errorDictionary = new Dictionary<string, string[]>(StringComparer.Ordinal);

            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    if (errors.Count == 1)
                    {
                        var errorMessage = HtmlEncoder.Default.Encode(errors[0].ErrorMessage);
                        errorDictionary.Add(key, new[] { errorMessage });
                    }
                    else
                    {
                        var errorMessages = new string[errors.Count];
                        for (var i = 0; i < errors.Count; i++)
                        {
                            errorMessages[i] = HtmlEncoder.Default.Encode(errors[i].ErrorMessage);
                        }

                        errorDictionary.Add(key, errorMessages);
                    }
                }
            }

            return errorDictionary;
        }
    }
}