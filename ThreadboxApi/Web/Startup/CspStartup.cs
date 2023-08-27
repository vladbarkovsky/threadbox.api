using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Web.Startup
{
    public class CspStartup
    {
        public static void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            app.UseCsp(options =>
            {
                if (webHostEnvironment.IsDevelopment())
                {
                    // Required for browser hot reload in development
                    // See: https://www.hanselman.com/blog/net-6-hot-reload-and-refused-to-connect-to-ws-because-it-violates-the-content-security-policy-directive-because-web-sockets
                    options.DefaultSources(s => s.Self()).ConnectSources(s => s.CustomSources("wss:"));
                }

                // Required for authorization silent renew using iframes
                options.FrameSources(s => s.Self());
                options.FrameSources(s => s.CustomSources(configuration[AppSettings.ClientBaseUrl]));
            });
        }
    }
}