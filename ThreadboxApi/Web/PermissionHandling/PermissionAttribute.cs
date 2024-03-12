using Microsoft.AspNetCore.Authorization;
using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.Web.PermissionHandling
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public PermissionAttribute(string permission)
        {
            Policy = $"{PermissionConstants.PermissionPolicyPrefix}.{permission}";
        }
    }
}