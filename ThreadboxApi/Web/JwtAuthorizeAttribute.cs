using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ThreadboxApi.Web
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public string Permission { get; set }

        public JwtAuthorizeAttribute()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}