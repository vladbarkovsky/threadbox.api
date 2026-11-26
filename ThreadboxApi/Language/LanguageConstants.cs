namespace ThreadboxApi.Language
{
    public class LanguageConstants
    {
        public const string DefaultLanguage = "en";

        /// <summary>
        /// <para>IETF BCP 47 language tags</para>
        /// <para>See: <see href="https://www.rfc-editor.org/info/bcp47/"/></para>
        /// </summary>
        public static IReadOnlySet<string> SupportedLanguages { get; } = new HashSet<string> { "en", "lv", "ru" };

        public const string LanguageCookieName = "language";
    }
}
