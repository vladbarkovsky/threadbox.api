using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi.ServiceLifetimes;

namespace ThreadboxApi
{
    public class ApplicationContext : IScoped
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// <see cref="IdentityUser{TKey}.Id"/> contained in the JWT access token.
        /// </summary>
        // IdentityServer stores user ID in subject claim
        // JWT specification: https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.2
        public string CurrentUserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
    }
}
