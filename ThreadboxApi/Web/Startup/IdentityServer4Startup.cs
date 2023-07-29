using IdentityServer4.Models;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Web.Startup
{
    public class IdentityServer4Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // See https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(IdentityResources)
                .AddInMemoryApiScopes(ApiScopes)
                .AddInMemoryClients(Clients)
                .AddAspNetIdentity<AppUser>();

            // IMPORTANT: Not recommended for production - we need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }

        private static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        private static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
        };

        private static IEnumerable<Client> Clients => new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())
                },

                AllowedScopes =
                {
                    "scope1"
                }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris =
                {
                    "https://localhost:5000/signin-oidc"
                },
                FrontChannelLogoutUri = "https://localhost:5000/signout-oidc",
                PostLogoutRedirectUris =
                {
                    "https://localhost:5000/signout-callback-oidc"
                },

                AllowOfflineAccess = true,
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    "scope2"
                }
            },
        };
    }
}