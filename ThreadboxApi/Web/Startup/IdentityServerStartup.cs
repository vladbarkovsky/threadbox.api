using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Web.Startup
{
    public class IdentityServerStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseNpgsql(
                            configuration.GetConnectionString(AppSettings.ConnectionStrings.Dev),
                            options => options.MigrationsAssembly("ThreadboxApi"));
                    };

                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30;
                })
                .AddInMemoryIdentityResources(IdentityResources)
                .AddInMemoryApiScopes(ApiScopes)
                .AddInMemoryClients(Clients)
                .AddAspNetIdentity<ApplicationUser>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }

        private static List<IdentityResource> IdentityResources => new()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        private static List<ApiScope> ApiScopes => new()
        {
            new ApiScope("threadbox_api", "Threadbox API"),
            new ApiScope(IdentityServerConstants.StandardScopes.OfflineAccess)
        };

        private static List<Client> Clients => new()
        {
            new Client
            {
                ClientId = "angular_client",
                ClientName = "Angular client",
                AllowedGrantTypes = GrantTypes.Code,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "threadbox_api"
                },

                RedirectUris = { "http://localhost:4200/is4/redirect-uri" },
                PostLogoutRedirectUris = { "http://localhost:4200" },
                AllowedCorsOrigins = { "http://localhost:4200" },
                AllowAccessTokensViaBrowser = false,
                AccessTokenLifetime = 3600,
                BackChannelLogoutUri = "https://localhost:5000/signout-callback-oidc",
                FrontChannelLogoutUri = "https://localhost:5000/signout-oidc",
                FrontChannelLogoutSessionRequired = true,
                AllowRememberConsent = false,
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowOfflineAccess = true,
                RequireClientSecret = false
            },
        };
    }
}