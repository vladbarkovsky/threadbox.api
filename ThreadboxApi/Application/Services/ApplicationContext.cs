using System.Security.Claims;
using ThreadboxApi.Application.Common.Interfaces;

namespace ThreadboxApi.Application.Services
{
    public class ApplicationContext : IScopedService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string BaseUrl => $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
        public string UserId => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}