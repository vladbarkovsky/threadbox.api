using System.Net.Mime;

namespace ThreadboxApi.Application.Common.Constants
{
    public class MediaConstants
    {
        public static HashSet<string> AllowedImageFormats { get; } = new()
        {
            MediaTypeNames.Image.Jpeg,
            MediaTypeNames.Image.Gif,
            "image/png",
            "image/bmp",
            "image/svg+xml"
        };
    }
}