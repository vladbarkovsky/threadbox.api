using Microsoft.AspNetCore.StaticFiles;

namespace ThreadboxApi.Application.Common
{
    public class ContentType
    {
        public static string Get(string fileName)
        {
            var contentTypeProvider = new FileExtensionContentTypeProvider();

            if (!contentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                throw new InvalidOperationException($"Cannot get Content-Type for file {fileName}");
            }

            return contentType;
        }
    }
}