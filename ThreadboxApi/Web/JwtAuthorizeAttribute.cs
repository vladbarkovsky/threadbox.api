using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ThreadboxApi.Web
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Permission { get; set; }

        public JwtAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
    }
}