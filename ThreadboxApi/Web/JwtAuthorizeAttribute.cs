using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ThreadboxApi.Web
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public JwtAuthorizeAttribute(string permission)
        {
            //AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;

            Policy = $"Permission.{permission}";
        }
    }
}