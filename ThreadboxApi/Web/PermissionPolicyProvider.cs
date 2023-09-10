using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Interfaces;

namespace ThreadboxApi.Web
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        { }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith("Permission"))
            {
                return await base.GetPolicyAsync(policyName);
            }

            var permission = policyName.Substring("Permission".Length + 1);
            var requirement = new PermissionRequirement(permission);
            return new AuthorizationPolicyBuilder().AddRequirements(requirement).Build();
        }
    }
}