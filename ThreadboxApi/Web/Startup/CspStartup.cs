using Microsoft.AspNetCore.Hosting;

namespace ThreadboxApi.Web.Startup
{
    public class CspStartup
    {
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                app.UseCsp(options =>
                {
                    // Required for browser HotReload in development
                    // See: https://www.hanselman.com/blog/net-6-hot-reload-and-refused-to-connect-to-ws-because-it-violates-the-content-security-policy-directive-because-web-sockets
                    options.DefaultSources(s => s.Self()).ConnectSources(s => s.CustomSources("wss:"));
                });
            }
        }
    }
}
