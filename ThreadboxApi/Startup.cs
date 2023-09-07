﻿using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Reflection;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Infrastructure.Identity;
using ThreadboxApi.Infrastructure.Persistence;
using ThreadboxApi.Web.ApiSpecification;

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
                    // Database for production
                }

                // Throw exceptions in case of performance issues with single queries
                // See https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
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
                        // Access-Control-Allow-Methods header content
                        // Source: https://stackoverflow.com/a/44385327
                        .WithMethods("PUT", "DELETE")
                        .WithHeaders("Authorization", "Content-Type")
                        .Build();
                });
            });

            if (_webHostEnvironment.IsDevelopment())
            {
                services.AddOpenApiDocument(settings =>
                {
                    settings.Title = "Threadbox API specification";

                    // Overrride default name generation patterns
                    settings.SchemaNameGenerator = new SchemaNameGenerator();

                    // JWT authorization (used for Swagger UI)
                    // Source: https://github.com/jasontaylordev/CleanArchitecture/blob/net6.0/src/WebUI/Startup.cs

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

                // NOTE: Must be declared through options.ExceptionHandler - otherwise it won't work
                options.ExceptionHandler = httpContext =>
                {
                    var exception = httpContext.Features.Get<IExceptionHandlerFeature>().Error;

                    if (exception is HttpResponseException)
                    {
                        var httpResponseException = exception as HttpResponseException;
                        httpContext.Response.StatusCode = httpResponseException.StatusCode;
                    }

                    return Task.CompletedTask;
                };
            });

            services
                .AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var identityServerBuilder = services.AddIdentityServer();

            if (_webHostEnvironment.IsDevelopment())
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }
            else
            {
                // Use SSL certificate
            }

            identityServerBuilder
                .AddAspNetIdentity<ApplicationUser>()
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseNpgsql(
                            _configuration.GetConnectionString(AppSettings.ConnectionStrings.Development),
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
                            _configuration[AppSettings.ClientBaseUrl]
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
                            _configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-in-redirect-callback",
                            _configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-in-silent-callback"
                        },

                        PostLogoutRedirectUris =
                        {
                            _configuration[AppSettings.ClientBaseUrl] + "/authorization/sign-out-redirect-callback"
                        },
                    },
                });

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(options =>
                {
                    // Authorization server base URL
                    // We can't get the base URL during services configuration, so it is hardcoded
                    options.Authority = _webHostEnvironment.IsDevelopment() ? "https://localhost:5000" : "https://threadbox.prod";
                    // API resource
                    options.Audience = "threadbox_api";
                });

            services.AddAuthorization();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            services.AddMvc();

            services.ConfigureApplicationCookie(options =>
            {
                //// Disable redirecting for unauthorized HTTP requests
                //options.Events.OnRedirectToLogin = context =>
                //{
                //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //    return Task.CompletedTask;
                //};

                //// Disable redirecting for forbidden HTTP requests
                //options.Events.OnRedirectToAccessDenied = context =>
                //{
                //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //    return Task.CompletedTask;
                //};
            });
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

            // Enable HTTP routing
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseCsp(options =>
            {
                if (_webHostEnvironment.IsDevelopment())
                {
                    // Required for Identity UI Razor pages that use browser hot reload in development
                    // See: https://www.hanselman.com/blog/net-6-hot-reload-and-refused-to-connect-to-ws-because-it-violates-the-content-security-policy-directive-because-web-sockets
                    options.DefaultSources(s => s.Self()).ConnectSources(s => s.CustomSources("wss:"));
                }

                // Required for IdentityServer4 iframes
                options.FrameSources(s => s.Self());
                options.FrameSources(s => s.CustomSources(_configuration[AppSettings.ClientBaseUrl]));
            });

            // Enable internet access to wwwroot
            app.UseStaticFiles();

            // Configure HTTP endpoints for controllers and Razor pages
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}