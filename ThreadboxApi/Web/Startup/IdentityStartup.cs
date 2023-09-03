using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Infrastructure.Identity;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Web.Startup
{
    public class IdentityStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

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
                            // TODO: Assembly name should not be hardcoded
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
                    new ApiScope("threadbox_api.access", "Threadbox API"),
                })
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource
                    {
                        Name = "threadbox_api",
                        Scopes = { "threadbox_api.access" }
                    }
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
                            "threadbox_api.access"
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
                .AddAspNetIdentity<ApplicationUser>()
                .AddJwtBearerClientAuthentication();

            //services.AddAuthentication().AddIdentityServerAuthentication();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    // base-address of your identityserver
                    options.Authority = "https://localhost:5000";

                    // name of the API resource
                    options.Audience = "threadbox_api";
                });

            services.AddAuthorization();
        }

        public static void Configure(IApplicationBuilder app)
        {
            //app.UseAuthentication();

            app.UseIdentityServer();

            /// IMPORTANT: This method must be called between
            /// <see cref="EndpointRoutingApplicationBuilderExtensions.UseRouting(IApplicationBuilder)"/> and
            /// <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>
            app.UseAuthorization();
        }
    }
}