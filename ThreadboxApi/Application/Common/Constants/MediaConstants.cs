using System.Net.Mime;

namespace ThreadboxApi.Application.Common.Constants
{
    public class MediaConstants
    {
        public static IReadOnlyCollection<string> AllowedImageFormats { get; } = new HashSet<string>
        {
            MediaTypeNames.Image.Jpeg,
            MediaTypeNames.Image.Gif,
            "image/png",
            "image/bmp",
            "image/svg+xml"
        };
    }
}