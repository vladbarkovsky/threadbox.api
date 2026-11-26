using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ThreadboxApi.Web.Language
{
    public class CulturePageRouteModelConvention : IPageRouteModelConvention
    {
        public void Apply(PageRouteModel model)
        {
            foreach (SelectorModel selector in model.Selectors)
            {
                AttributeRouteModel routeModel = selector.AttributeRouteModel;

                if (routeModel == null)
                {
                    continue;
                }

                string template = routeModel.Template ?? string.Empty;

                if (template.StartsWith('/'))
                {
                    template = template.Substring(1);
                }

                routeModel.Template = AttributeRouteModel.CombineTemplates("{culture:culture}", template);
            }
        }
    }
}
