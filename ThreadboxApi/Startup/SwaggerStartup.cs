using NSwag.Generation.Processors.Security;
using NSwag;

namespace ThreadboxApi.Startup
{
    public class SwaggerStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "Threadbox API";

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
                app.UseOpenApi();

                app.UseSwaggerUi3(settings =>
                {
                    settings.Path = "/swagger";
                });
            }
        }
    }
}