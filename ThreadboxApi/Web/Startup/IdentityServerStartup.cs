using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Web.Startup
{
    public class IdentityServerStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            var identityServerBuilder = services.AddIdentityServer();

            if (webHostEnvironment.IsDevelopment())
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                // Use SSL certificate
            }

            identityServerBuilder
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseNpgsql(
                            configuration.GetConnectionString(AppSettings.ConnectionStrings.Development),
                            options => options.MigrationsAssembly("ThreadboxApi"));
                    };

                    options.EnableTokenCleanup = true;
                })
                .AddInMemoryIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                })
                .AddInMemoryApiScopes(new List<ApiScope>
                {
                    new ApiScope("threadbox_api", "Threadbox API"),
                })
                .AddInMemoryClients(new List<Client>
                {
                    new Client
                    {
                        ClientId = "angular_client",
                        ClientName = "Threadbox",

                        AllowedCorsOrigins =
                        {
                            configuration[AppSettings.ClientBaseUrl]
                        },

                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        RequireClientSecret = false,
                        AllowOfflineAccess = true,

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.OfflineAccess,
                            "threadbox_api"
                        },

                        RedirectUris =
                        {
                            configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-in-redirect-callback",
                            configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-in-silent-callback"
                        },

                        PostLogoutRedirectUris =
                        {
                            configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-out-redirect-callback"
                        },
                    },
                })
                .AddAspNetIdentity<ApplicationUser>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}