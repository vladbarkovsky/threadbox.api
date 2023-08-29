using NSwag.Generation.Processors.Security;
using NSwag;
using ThreadboxApi.Web.ApiSpecification;

namespace ThreadboxApi.Web.Startup
{
    public class SwaggerStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddOpenApiDocument(settings =>
            {
                settings.Title = "Threadbox API";
                settings.SchemaNameGenerator = new SchemaNameGenerator();

                // TODO: IdentityServer4 authorization
                // Authorization
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