using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Infrastructure.Identity;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Web.Startup
{
    public class IdentityStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddIdentityServer(options =>
                {
                    options.UserInteraction = new UserInteractionOptions()
                    {
                        LogoutUrl = "/Account/Logout",
                        LoginUrl = "/Account/Login",
                        LoginReturnUrlParameter = "returnUrl"
                    };

                    options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                })
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

            services.AddAuthentication();
            services.AddAuthorization();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private static List<IdentityResource> IdentityResources => new()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        private static List<ApiScope> ApiScopes => new()
        {
            new ApiScope("threadbox_api", "Threadbox API"),
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
                    "threadbox_api"
                },

                RedirectUris = { "http://localhost:4200" },
                PostLogoutRedirectUris = { "http://localhost:4200" },
                AllowedCorsOrigins = { "http://localhost:4200" },
                AllowAccessTokensViaBrowser = false,
                AccessTokenLifetime = 3600,
                BackChannelLogoutUri = "https://localhost:5000/signout-callback-oidc",
                FrontChannelLogoutUri = "https://localhost:5000/signout-oidc",
                FrontChannelLogoutSessionRequired = true,
                AllowRememberConsent = false,
                ClientSecrets = { new Secret("your_client_secret".Sha256()) },
                RequirePkce = true,
                AllowPlainTextPkce = false
            },
        };
    }
}