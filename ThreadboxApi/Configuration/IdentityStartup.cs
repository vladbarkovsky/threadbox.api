using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.Bff;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Configuration
{
    public class IdentityStartup
    {
        public static void ConfigureServices(IServiceCollection services, AppSettings appSettings, IWebHostEnvironment webHostEnvironment)
        {
            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var identityServerBuilder = services
                .AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryIdentityResources(new IdentityResource[]
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                })
                .AddInMemoryApiResources(new ApiResource[]
                {
                    new ApiResource("threadbox_api", "Threadbox API")
                    {
                        Scopes = { "threadbox_api.access" }
                    }
                })
                .AddInMemoryClients(new Client[]
                {
                    new Client
                    {
                        ClientId = "bff",
                        ClientName = "BFF",
                        AllowedGrantTypes = GrantTypes.Code,
                        AllowOfflineAccess = true,

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.OfflineAccess,
                            "threadbox_api.access"
                        },

                        PostLogoutRedirectUris =
                        {
                            appSettings.BaseUrl + "/bff/sign-out-redirect-callback"
                        },

                        RedirectUris =
                        {
                            appSettings.BaseUrl + "/bff/sign-in-redirect-callback",
                        },

                        RequirePkce = false,

                        ClientSecrets =
                        {
                            new Secret(appSettings.OidcBffClientSecret.Sha256())
                        },

                        AccessTokenLifetime = 1_800,
                        AbsoluteRefreshTokenLifetime = appSettings.AbsoluteRefreshTokenLifetimeSeconds
                    }
                })
                .AddOperationalStore<ApplicationDbContext>(options =>
                {
                    options.EnableTokenCleanup = true;
                });

            if (webHostEnvironment.IsDevelopment())
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                identityServerBuilder.AddSigningCredential(new X509Certificate2("/certs/cert.pfx", appSettings.SslPassword));
            }

            // Disabling JWT token claims mapping by ASP.NET Identity.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication().AddIdentityServerJwt();
            services.AddAuthorization();

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<AccessTokenRefreshMiddleware>();
            app.UseMiddleware<AccessTokenMiddleware>();
            app.UseAuthentication();
            app.UseMiddleware<PermissionMiddleware>();
            app.UseIdentityServer();
            app.UseAuthorization();
        }
    }
}