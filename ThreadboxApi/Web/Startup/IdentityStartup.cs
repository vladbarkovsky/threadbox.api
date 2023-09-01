using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Infrastructure.Identity;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Web.Startup
{
    public class IdentityStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            /// IMPORTANT: This method must be called between
            /// <see cref="EndpointRoutingApplicationBuilderExtensions.UseRouting(IApplicationBuilder)"/> and
            /// <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>
            app.UseAuthorization();
        }
    }
}