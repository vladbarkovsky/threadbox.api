using Askmethat.Aspnet.JsonLocalizer.Extensions;
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
            services.AddJsonLocalization(options =>
            {
                options.ResourcesPath = @"Application\Common\Translations";
                options.LocalizationMode = Askmethat.Aspnet.JsonLocalizer.JsonOptions.LocalizationMode.I18n;
                options.SupportedCultureInfos = SupportedLanguages.Select(x => new CultureInfo(x)).ToHashSet();
                options.DefaultCulture = new CultureInfo(DefaultLanguage);
                options.DefaultUICulture = new CultureInfo(DefaultLanguage);
            });
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseRequestLocalization(options =>
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