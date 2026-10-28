using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Net.Http.Headers;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Configuration
{
    public class SecurityStartup
    {
        public static void ConfigureServices(IServiceCollection services, AppSettings appSettings, IWebHostEnvironment webHostEnvironment)
        {
            services.AddCors(void (CorsOptions options) =>
            {
                options.AddDefaultPolicy(void (CorsPolicyBuilder policyBuilder) =>
                {
                    policyBuilder
                        .WithOrigins(appSettings.FrontendBaseUrl)
                        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
                        .WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType)
                        .AllowCredentials()
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

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (!webHostEnvironment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
                context.Response.Headers["X-Frame-Options"] = "DENY";

                await next();
            });
        }

        public static void ConfigureCors(IApplicationBuilder app)
        {
            app.UseCors();
        }
    }
}
