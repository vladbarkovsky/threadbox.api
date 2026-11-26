using ThreadboxApi.Language;

namespace ThreadboxApi.Web.Language
{
    public static class RequestLanguage
    {
        public static string Get(HttpRequest request)
        {
            if (
                request.Cookies.TryGetValue(LanguageConstants.LanguageCookieName, out string language) &&
                LanguageConstants.SupportedLanguages.Contains(language))
            {
                return language;
            }

            return LanguageConstants.DefaultLanguage;
        }
    }
}
