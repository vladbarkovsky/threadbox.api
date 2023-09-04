using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ThreadboxApi.Web
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public JwtAuthorizeAttribute()
        {
            // NOTE: The only way to get JWT authentication to work is to set
            // the authentication scheme directly to the attribute.
            // Other approaches either don't work or break Identity UI endpoints
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}