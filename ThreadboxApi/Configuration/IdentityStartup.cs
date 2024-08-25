using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Configuration
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

            var identityServerBuilder = services
            .AddIdentityServer()
            .AddAspNetIdentity<ApplicationUser>()
            .AddApiResources()
            .AddIdentityResources()
            .AddClients()
            .AddOperationalStore<ApplicationDbContext>(options =>
            {
                options.EnableTokenCleanup = true;
            })
            .AddResourceStore<InMemoryResourcesStore>()
            .AddClientStore<InMemoryClientStore>();

            if (webHostEnvironment.IsDevelopment())
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                // Use SSL certificate.
            }

            services.Configure<ApiAuthorizationOptions>(options =>
            {
                options.ApiScopes = new ApiScopeCollection(new List<ApiScope>
                {
                    new ApiScope("threadbox_api.access", "Threadbox API access"),
                });

                options.ApiResources = new ApiResourceCollection(new List<ApiResource>
                {
                    /// Based on <see cref="ApiResourceCollection.AddIdentityServerJwt(string, Action{ApiResourceBuilder})"/>
                    new ApiResource
                    {
                        /// Required for <see cref="AuthenticationBuilderExtensions.AddIdentityServerJwt(AuthenticationBuilder)"/>
                        Name = webHostEnvironment.ApplicationName + "API",

                        DisplayName = "Threadbox API",
                            Scopes = { "threadbox_api.access" },

                        Properties =
                        {
                            { "Clients", "angular_client" },
                        }
                    }
                });

                options.IdentityResources = new IdentityResourceCollection(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile()
                });

                options.Clients = new ClientCollection(new List<Client>
                {
                    /// Based on <see cref="ClientCollection.AddSPA(string, Action{ClientBuilder})"/>
                    new Client
                    {
                        ClientId = "angular_client",
                        ClientName = "Angular client",
                        AllowedCorsOrigins = { configuration[AppSettings.ClientBaseUrl] },
                        AllowedGrantTypes = GrantTypes.Code,

                        AllowedScopes =
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,

                            // This scope is required for silent renew through refresh tokens.
                            // Regardless of this scope client still uses iframe for silent renew.
                            // TODO: Investigate iframe and refresh token security aspects and choose more appropriate
                            // silent renew mechanism. If it will be refresh tokens, fix problem described above.
                            // IdentityServerConstants.StandardScopes.OfflineAccess,

                            "threadbox_api.access"
                        },

                        AllowOfflineAccess = true,

                        PostLogoutRedirectUris =
                        {
                            configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-out-redirect-callback"
                        },

                        RedirectUris =
                        {
                            configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-in-redirect-callback",
                            configuration[AppSettings.ClientBaseUrl] + "/assets/authorization/sign-in-silent-callback.html"
                        },

                        Properties =
                        {
                            { "Profile", "SPA" }
                        },

                        AllowAccessTokensViaBrowser = true,
                        ProtocolType = IdentityServerConstants.ProtocolTypes.OpenIdConnect,
                        RefreshTokenExpiration = TokenExpiration.Absolute,
                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                        RequireClientSecret = false
                    }
                });
            });

            // Disabling JWT token claims mapping by ASP.NET Identity.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication().AddIdentityServerJwt();
            services.AddAuthorization();

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseMiddleware<PermissionMiddleware>();
            app.UseIdentityServer();
            app.UseAuthorization();
        }
    }
}