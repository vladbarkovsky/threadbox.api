using Microsoft.AspNetCore.Localization.Routing;
using ThreadboxApi.Language;
using ThreadboxApi.Web.Language;

namespace ThreadboxApi.Configuration
{
    public static class LocalizationStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseRequestLocalization(void (RequestLocalizationOptions options) =>
            {
                options.RequestCultureProviders.Clear();

                options.RequestCultureProviders.Add(new RouteDataRequestCultureProvider
                {
                    RouteDataStringKey = "culture",
                    UIRouteDataStringKey = "culture"
                });

                options.RequestCultureProviders.Add(new ApplicationRequestCultureProvider());
                string[] supportedLanguages = LanguageConstants.SupportedLanguages.ToArray();

                options
                    .AddSupportedCultures(supportedLanguages)
                    .AddSupportedUICultures(supportedLanguages)
                    .SetDefaultCulture(LanguageConstants.DefaultLanguage);
            });
        }
    }
}
