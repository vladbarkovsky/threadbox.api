using NSwag;
using NSwag.Generation.Processors.Security;
using ThreadboxApi.Web;

namespace ThreadboxApi.Configuration
{
    public class NSwagStartup
    {
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment webHostEnvironment)
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

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.UseSwaggerUi3(settings =>
            {
                var prefix = webHostEnvironment.IsDevelopment() ? string.Empty : "/threadbox-api";

                settings.Path = "/threadbox-api/api";
                settings.DocumentPath = "/threadbox-api/api/specification.json";
                //settings.TransformToExternalPath = (internalPath, request) => request.PathBase + internalPath;
            });
        }
    }
}