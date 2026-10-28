using Askmethat.Aspnet.JsonLocalizer.Extensions;
using Askmethat.Aspnet.JsonLocalizer.JsonOptions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace ThreadboxApi.Configuration
{
    public static class LocalizationStartup
    {
        public const string DefaultLanguage = "en";

        /// <summary>
        /// <para>IETF BCP 47 language tags</para>
        /// <para>See: <see href="https://www.rfc-editor.org/info/bcp47/"/></para>
        /// </summary>
        private static IReadOnlySet<string> SupportedLanguages { get; }

        static LocalizationStartup()
        {
            SupportedLanguages = new HashSet<string> { "en", "lv", "ru" };
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddJsonLocalization(void (JsonLocalizationOptions options) =>
            {
                options.ResourcesPath = @"Translations";
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