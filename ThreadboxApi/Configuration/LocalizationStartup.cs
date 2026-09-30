using Askmethat.Aspnet.JsonLocalizer.Extensions;
using Askmethat.Aspnet.JsonLocalizer.JsonOptions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace ThreadboxApi.Configuration
{
    public class LocalizationStartup
    {
        public const string DefaultLanguage = "en";
        public static IReadOnlySet<string> SupportedLanguages { get; } = new HashSet<string> { "en", "lv", "ru" };

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddJsonLocalization(void (JsonLocalizationOptions options) =>
            {
                options.ResourcesPath = @"Application\Common\Translations";
                options.LocalizationMode = LocalizationMode.I18n;
                options.SupportedCultureInfos = SupportedLanguages.Select(CultureInfo (string language) => new CultureInfo(language)).ToHashSet();
                options.DefaultCulture = new CultureInfo(DefaultLanguage);
                options.DefaultUICulture = new CultureInfo(DefaultLanguage);
            });
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseRequestLocalization(void (RequestLocalizationOptions options) =>
            {
                options
                    .AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider())
                    .AddSupportedCultures(SupportedLanguages.ToArray())
                    .AddSupportedUICultures(SupportedLanguages.ToArray())
                    .SetDefaultCulture(DefaultLanguage);
            });
        }
    }
}