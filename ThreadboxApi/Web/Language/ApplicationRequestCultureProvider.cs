using Microsoft.AspNetCore.Localization;

namespace ThreadboxApi.Web.Language
{
    public class ApplicationRequestCultureProvider : RequestCultureProvider
    {
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            string language = RequestLanguage.Get(httpContext.Request);
            return Task.FromResult(new ProviderCultureResult(culture: language, uiCulture: language));
        }
    }
}
