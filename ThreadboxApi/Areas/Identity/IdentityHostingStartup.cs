[assembly: HostingStartup(typeof(ThreadboxApi.Areas.Identity.IdentityHostingStartup))]

namespace ThreadboxApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }
    }
}