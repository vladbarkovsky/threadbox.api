using Microsoft.Net.Http.Headers;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Configuration
{
    public class SecurityStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(configuration[AppSettings.ClientBaseUrl])
                        .WithMethods("PUT", "DELETE")
                        .WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType)
                        .Build();
                });
            });

            if (!webHostEnvironment.IsDevelopment())
            {
                services.AddHsts(options =>
                {
                    options.MaxAge = TimeSpan.FromDays(365);
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                });
            }
        }

        public static void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(options => options.NoReferrer());
            app.UseXXssProtection(options => options.Disabled());
            app.UseXfo(options => options.Deny());

            app.UseCsp(options =>
            {
                options
                    .DefaultSources(s => s.None())
                    .BlockAllMixedContent()
                    // Razor styles, Swagger UI styles
                    .StyleSources(s => s.Self().CustomSources("sha256-pyVPiLlnqL9OWVoJPs/E6VVF5hBecRzM2gBiarnaqAo="))
                    // Images for Razor, images for Swagger UI
                    .ImageSources(s => s.Self().CustomSources("data:"))
                    // Sending forms to server from Identity UI, sending forms to client from Identity UI
                    .FormActions(s => s.Self().CustomSources(configuration[AppSettings.ClientBaseUrl]))
                    // iframe on client for authorization silent renew
                    .FrameAncestors(s => s.CustomSources(configuration[AppSettings.ClientBaseUrl]))

                    .ScriptSources(s => s.Self().CustomSources(
                        // Script in iframe on client for authorization silent renew
                        "sha256-fa5rxHhZ799izGRP38+h4ud5QXNT0SFaFlh4eqDumBI=",
                        // Script in Swagger UI
                        "sha256-jYwH+ovNhdZXLQSoSAgcVH3aaKh7DqPa3Z3LJO3icXE="));

                if (webHostEnvironment.IsDevelopment())
                {
                    // Browser Link, Live Reload, etc.
                    options.ConnectSources(s => s.CustomSources("ws://localhost:*", "http://localhost:*"));
                }

                options.ReportUris(s => s.Uris("/api/Csp/CreateCspReport"));
            });

            if (!webHostEnvironment.IsDevelopment())
            {
                app.UseHsts();
            }
        }
    }
}