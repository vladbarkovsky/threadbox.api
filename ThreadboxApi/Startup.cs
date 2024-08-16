using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Net.Http.Headers;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ApiSpecification;
using ThreadboxApi.Web.Exceptions;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    options.UseNpgsql(_configuration.GetConnectionString(AppSettings.ConnectionStrings.Development));
                }
                else
                {
                    // Database for production.
                }

                // Throw exceptions in case of performance issues with single queries.
                // See https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries.
                options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });

            services.Scan(scan => scan.FromAssemblyOf<Program>()
                .AddClasses(x => x.AssignableTo<ISingletonService>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime()
                .AddClasses(x => x.AssignableTo<IScopedService>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
                .AddClasses(x => x.AssignableTo<ITransientService>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());

            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(_configuration[AppSettings.ClientBaseUrl])
                        // NOTE: CORS allows simple methods (GET, HEAD, POST) regardless of
                        // Access-Control-Allow-Methods header content.
                        // Source: https://stackoverflow.com/a/44385327.
                        .WithMethods("PUT", "DELETE")
                        .WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType)
                        .Build();
                });
            });

            if (_webHostEnvironment.IsDevelopment())
            {
                services.AddOpenApiDocument(settings =>
                {
                    settings.Title = "Threadbox API specification";

                    // Overriding default name generation patterns.
                    settings.SchemaNameGenerator = new SchemaNameGenerator();

                    // JWT authorization (used for Swagger UI).
                    // Source: https://github.com/jasontaylordev/CleanArchitecture/blob/net6.0/src/WebUI/Startup.cs.

                    settings.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}."
                    });

                    settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                });
            }

            services.AddExceptionHandler(options =>
            {
                options.AllowStatusCode404Response = true;

                /// NOTE: Must be declared through <see cref="ExceptionHandlerOptions.ExceptionHandler"/> - otherwise it won't work.
                options.ExceptionHandler = async httpContext =>
                {
                    var exception = httpContext.Features.Get<IExceptionHandlerFeature>().Error;

                    if (exception is HttpStatusException)
                    {
                        httpContext.Response.StatusCode = (exception as HttpStatusException).StatusCode;

                        if (exception is HttpResponseException)
                        {
                            // TODO: Add localization of exception messages.
                            await httpContext.Response.WriteAsync((exception as HttpResponseException).Response);
                        }
                    }
                };
            });

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

            if (_webHostEnvironment.IsDevelopment())
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                // Use SSL certificate.
            }

            /// NOTE: Configuration is based on <see cref="ApiAuthorizationOptions"/> methods.
            services.Configure<ApiAuthorizationOptions>(options =>
            {
                options.ApiScopes = new ApiScopeCollection(new List<ApiScope>
                {
                    new ApiScope("threadbox_api.access", "Threadbox API access"),
                });

                options.ApiResources = new ApiResourceCollection(new List<ApiResource>
                {
                    new ApiResource
                    {
                        /// Required for <see cref="AuthenticationBuilderExtensions.AddIdentityServerJwt(AuthenticationBuilder)"/>
                        Name = _webHostEnvironment.ApplicationName + "API",

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

                /// Based on <see cref="ClientCollection.AddSPA(string, Action{ClientBuilder})"/>
                options.Clients = new ClientCollection(new List<Client>
                {
                    new Client
                    {
                        ClientId = "angular_client",
                        ClientName = "Angular client",
                        AllowedCorsOrigins = { _configuration[AppSettings.ClientBaseUrl] },
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
                            _configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-out-redirect-callback"
                        },

                        RedirectUris =
                        {
                            _configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-in-redirect-callback",
                            _configuration[AppSettings.ClientBaseUrl] + "/assets/authorization/sign-in-silent-callback.html"
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

            // Disable JWT token claims mapping by ASP.NET Identity.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication().AddIdentityServerJwt();
            services.AddAuthorization();

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors();

            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseSwaggerUi3(settings =>
                {
                    settings.Path = "/api";
                    settings.DocumentPath = "/api/specification.json";
                });
            }

            app.UseExceptionHandler();

            // Enable HTTP routing.
            app.UseRouting();

            app.UseAuthentication();
            app.UseMiddleware<PermissionsMiddleware>();
            app.UseIdentityServer();
            app.UseAuthorization();

            // Enable internet access to wwwroot.
            app.UseStaticFiles();

            // Configure HTTP endpoints for controllers and Razor pages.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}