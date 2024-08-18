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

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.UseCors();
            app.UseHttpsRedirection();

            if (!webHostEnvironment.IsDevelopment())
            {
                app.UseHsts();
            }
        }
    }
}