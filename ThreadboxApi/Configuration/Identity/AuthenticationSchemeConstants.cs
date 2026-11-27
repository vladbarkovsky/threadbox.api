using OpenIddict.Validation.AspNetCore;

namespace ThreadboxApi.Configuration.Identity
{
    public static class AuthenticationSchemeConstants
    {
        public const string Forwarding = "Threadbox";
        public const string AccessToken = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    }
}
