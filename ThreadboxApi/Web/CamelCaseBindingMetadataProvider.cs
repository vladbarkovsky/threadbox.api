using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Text.Json;

namespace ThreadboxApi.Web
{
    public class CamelCaseBindingMetadataProvider : IBindingMetadataProvider
    {
        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
            if (context.Key.MetadataKind != ModelMetadataKind.Property || string.IsNullOrWhiteSpace(context.Key.Name))
            {
                return;
            }

            string containerNamespace = context.Key.ContainerType?.Namespace;

            if (containerNamespace == null || !containerNamespace.StartsWith("ThreadboxApi.Application"))
            {
                return;
            }

            if (!string.IsNullOrEmpty(context.BindingMetadata.BinderModelName))
            {
                return;
            }

            context.BindingMetadata.BinderModelName = JsonNamingPolicy.CamelCase.ConvertName(context.Key.Name);
        }
    }
}
