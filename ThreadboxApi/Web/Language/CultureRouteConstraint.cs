using ThreadboxApi.Configuration;
using ThreadboxApi.Language;

namespace ThreadboxApi.Web.Language
{
    public class CultureRouteConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out object culture) && culture is string cultureName)
            {
                return LanguageConstants.SupportedLanguages.Contains(cultureName);
            }

            return false;
        }
    }
}
