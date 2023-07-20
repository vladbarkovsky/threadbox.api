using System.Net.Mime;

namespace ThreadboxApi.Application.Common.Constants
{
    public class MediaConstants
    {
        public static List<AllowedImage> AllowedImages { get; } = new()
        {
            new AllowedImage { ContentType = MediaTypeNames.Image.Jpeg, Extension = ".jpeg" },
            new AllowedImage { ContentType = MediaTypeNames.Image.Jpeg, Extension = ".jpg" },
            new AllowedImage { ContentType = MediaTypeNames.Image.Gif, Extension = ".gif" },
            new AllowedImage { ContentType = "image/png", Extension = ".png" },
            new AllowedImage { ContentType = "image/bmp", Extension = ".bmp" },
            new AllowedImage { ContentType = "image/svg+xml", Extension = ".svg" }
        };

        public class AllowedImage
        {
            public string ContentType { get; set; }
            public string Extension { get; set; }
        }
    }
}