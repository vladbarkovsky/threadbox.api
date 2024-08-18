using Microsoft.AspNetCore.StaticFiles;

namespace ThreadboxApi.Application.Common
{
    public static class ContentType
    {
        private static FileExtensionContentTypeProvider FileExtensionContentTypeProvider { get; }

        static ContentType()
        {
            FileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
        }

        public static string Get(string fileName)
        {
            if (FileExtensionContentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                return contentType;
            }

            throw new InvalidOperationException($"Cannot get Content-Type for file {fileName}");
        }
    }
}