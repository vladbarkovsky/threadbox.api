using Askmethat.Aspnet.JsonLocalizer.Extensions;
using Askmethat.Aspnet.JsonLocalizer.JsonOptions;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Globalization;
using ThreadboxApi.Language;
using ThreadboxApi.Web.Language;

namespace ThreadboxApi.Configuration
{
    public static class LocalizationStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddJsonLocalization(void (JsonLocalizationOptions options) =>
            {
                options.ResourcesPath = @"Translations";
                options.LocalizationMode = LocalizationMode.I18n;

                options.SupportedCultureInfos = LanguageConstants.SupportedLanguages
                    .Select(CultureInfo (string language) => new CultureInfo(language))
                    .ToHashSet();

                options.DefaultCulture = new CultureInfo(LanguageConstants.DefaultLanguage);
                options.DefaultUICulture = new CultureInfo(LanguageConstants.DefaultLanguage);
            });

            services.AddSingleton<IHtmlLocalizerFactory, HtmlLocalizerFactory>();
            services.AddTransient<IViewLocalizer, ViewLocalizer>();
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