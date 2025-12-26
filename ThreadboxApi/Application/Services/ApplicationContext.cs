using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Application.Services
{
    public class ApplicationContext : IScopedService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Base URL of API
        /// </summary>
        public string BaseUrl
        {
            get
            {
                var request = _httpContextAccessor.HttpContext.Request;
                return $"{request.Scheme}://{request.Host.ToUriComponent()}";
            }
        }

        /// <summary>
        /// <see cref="IdentityUser{TKey}.Id"/> contained in the authorization token.
        /// </summary>
        // IdentityServer stores user ID in subject claim
        // JWT specification: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.2
        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
    }
}