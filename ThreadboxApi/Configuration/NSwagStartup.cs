using NSwag;
using NSwag.Generation.Processors.Security;
using ThreadboxApi.Web.ApiSpecification;

namespace ThreadboxApi.Configuration
{
    public class NSwagStartup
    {
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
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
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                app.UseSwaggerUi3(settings =>
                {
                    settings.Path = "/api";
                    settings.DocumentPath = "/api/specification.json";
                });
            }
        }
    }
}