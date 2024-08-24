using Microsoft.AspNetCore.Builder;
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
                    .BlockAllMixedContent()
                    .StyleSources(s => s.Self().UnsafeInline())
                    .FontSources(s => s.Self())
                    .ImageSources(s => s.Self())
                    .FormActions(s => s.Self().CustomSources(configuration[AppSettings.ClientBaseUrl]))
                    .FrameSources(s => s.Self())
                    .FrameAncestors(s => s.Self().CustomSources(configuration[AppSettings.ClientBaseUrl]))
                    .ScriptSources(s => s.Self().CustomSources("sha256-fa5rxHhZ799izGRP38+h4ud5QXNT0SFaFlh4eqDumBI="));

                if (webHostEnvironment.IsDevelopment())
                {
                    options.DefaultSources(s => s.Self().CustomSources("ws://localhost:*", "http://localhost:*"));
                }
            });

            if (!webHostEnvironment.IsDevelopment())
            {
                app.UseHsts();
            }
        }
    }
}