using Microsoft.Net.Http.Headers;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Configuration
{
    public class SecurityStartup
    {
        public static void ConfigureServices(IServiceCollection services, AppSettings appSettings, IWebHostEnvironment webHostEnvironment)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(appSettings.ClientBaseUrl)
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

        public static void Configure(IApplicationBuilder app, AppSettings appSettings, IWebHostEnvironment webHostEnvironment)
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
                    // Swagger UI styles
                    .StyleSources(s => s.Self().CustomSources("sha256-pyVPiLlnqL9OWVoJPs/E6VVF5hBecRzM2gBiarnaqAo="))
                    // Images for Swagger UI
                    .ImageSources(s => s.Self().CustomSources("data:"))

                    .ScriptSources(s => s.Self().CustomSources(
                        // Script in Swagger UI
                        "sha256-jYwH+ovNhdZXLQSoSAgcVH3aaKh7DqPa3Z3LJO3icXE="));

                if (webHostEnvironment.IsDevelopment())
                {
                    // TODO: Check if required.
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