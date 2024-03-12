using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Identity.Permissions;

namespace ThreadboxApi.Web.PermissionHandling
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        { }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PermissionConstants.PermissionPolicyPrefix))
            {
                return await base.GetPolicyAsync(policyName);
            }

            var permission = policyName[(PermissionConstants.PermissionPolicyPrefix.Length + 1)..];
            var requirement = new PermissionRequirement(permission);

            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }
    }
}