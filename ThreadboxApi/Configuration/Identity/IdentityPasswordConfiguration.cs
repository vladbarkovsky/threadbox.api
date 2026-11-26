using Microsoft.AspNetCore.Identity;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Configuration.Identity
{
    public static class IdentityPasswordConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(void (IdentityOptions options) =>
            {
                options.Password.RequiredLength = PasswordConstants.MinimumLength;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            services.AddScoped<IPasswordValidator<ApplicationUser>, MaximumPasswordLengthValidator>();
        }

    }
}
