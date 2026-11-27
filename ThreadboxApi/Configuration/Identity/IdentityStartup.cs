using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using System.Security.Cryptography.X509Certificates;
using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web.Bff;
using ThreadboxApi.Web.Constants;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Configuration.Identity
{
    public class IdentityStartup
    {
        public static void ConfigureServices(IServiceCollection services, AppSettings appSettings, IWebHostEnvironment webHostEnvironment)
        {
            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            IdentityPasswordConfiguration.Configure(services);

            services.ConfigureApplicationCookie(void (CookieAuthenticationOptions options) =>
            {
                options.LoginPath = UrlConstants.SignInPath;
                options.ReturnUrlParameter = "returnUrl";
                options.SlidingExpiration = false;

                options.Events.OnValidatePrincipal = async (CookieValidatePrincipalContext context) =>
                {
                    await SecurityStampValidator.ValidatePrincipalAsync(context);

                    if (context.Principal != null)
                    {
                        context.ShouldRenew = false;
                    }
                };
            });

            JsonWebTokenHandler.DefaultMapInboundClaims = false;

            services.AddOpenIddict()
                .AddCore(void (OpenIddictCoreBuilder builder) =>
                {
                    builder.UseEntityFrameworkCore()
                        .UseDbContext<ApplicationDbContext>();
                })
                .AddServer(void (OpenIddictServerBuilder builder) =>
                {
                    builder.SetIssuer(appSettings.BaseUrl);
                    builder.SetAuthorizationEndpointUris(UrlConstants.ConnectAuthorizePath);
                    builder.SetTokenEndpointUris(UrlConstants.ConnectTokenPath);
                    builder.AllowAuthorizationCodeFlow();
                    builder.RequireProofKeyForCodeExchange();
                    builder.AllowRefreshTokenFlow();
                    builder.RegisterScopes(
                        OpenIddictConstants.Scopes.OpenId,
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.OfflineAccess,
                        ApplicationOidcConstants.ApiScope);
                    builder.SetAccessTokenLifetime(TimeSpan.FromSeconds(appSettings.AccessTokenLifetimeSeconds));
                    builder.SetRefreshTokenLifetime(TimeSpan.FromSeconds(appSettings.AbsoluteRefreshTokenLifetimeSeconds));
                    builder.DisableRollingRefreshTokens();
                    builder.DisableSlidingRefreshTokenExpiration();

                    if (webHostEnvironment.IsDevelopment())
                    {
                        builder.AddDevelopmentEncryptionCertificate();
                        builder.AddDevelopmentSigningCertificate();
                    }
                    else
                    {
                        X509Certificate2 certificate = new X509Certificate2(
                            fileName: "/certs/signing.pfx",
                            password: appSettings.SigningPfxPassword,
                            keyStorageFlags: X509KeyStorageFlags.EphemeralKeySet);

                        builder.AddEncryptionCertificate(certificate);
                        builder.AddSigningCertificate(certificate);
                    }

                    builder.UseAspNetCore()
                        .EnableAuthorizationEndpointPassthrough();
                })
                .AddValidation(void (OpenIddictValidationBuilder builder) =>
                {
                    builder.UseLocalServer();
                    builder.UseAspNetCore();
                    builder.AddAudiences(ApplicationOidcConstants.ApiAudience);
                    builder.Configure(void (OpenIddictValidationOptions validationOptions) =>
                    {
                        validationOptions.TokenValidationParameters.AuthenticationType = AuthenticationSchemeConstants.AccessToken;
                    });
                });

            services
                .AddAuthentication(void (AuthenticationOptions options) =>
                {
                    options.DefaultScheme = AuthenticationSchemeConstants.Forwarding;
                })
                .AddPolicyScheme(
                    authenticationScheme: AuthenticationSchemeConstants.Forwarding,
                    displayName: null,
                    configureOptions: void (PolicySchemeOptions options) =>
                    {
                        options.ForwardDefaultSelector = string (HttpContext context) =>
                        {
                            if (context.Request.Path.StartsWithSegments("/Identity", StringComparison.OrdinalIgnoreCase))
                            {
                                return IdentityConstants.ApplicationScheme;
                            }

                            return AuthenticationSchemeConstants.AccessToken;
                        };
                    });

            services
                .AddAuthorization()
                .AddSingleton<IAuthorizationHandler, PermissionHandler>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app
                .UseMiddleware<AccessTokenRefreshMiddleware>()
                .UseMiddleware<AccessTokenMiddleware>()
                .UseAuthentication()
                .UseMiddleware<PermissionMiddleware>()
                .UseAuthorization();
        }
    }
}
