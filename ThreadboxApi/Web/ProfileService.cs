using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Web
{
    public class ProfileService : ITransientService, IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(userId);
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var claims = principal.Claims.Where(x => context.RequestedClaimTypes.Contains(x.Type));
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                claims.Append(new Claim(ClaimTypes.Role, roles.First()));
            }

            context.IssuedClaims = claims.ToList();
            return;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}