using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Configuration
{
    public class Constants
    {
        /// <summary>
        /// HTTP request URL to get <see cref="Domain.Entities.ThreadImage"/> as file <br/>
        /// Use <see cref="string.Format(string, object?[])"/> <br/>
        /// {0} - <see cref="ThreadImage.Id"/>
        /// </summary>
        public const string ThreadImageRequest = "/ThreadImages/GetThreadImage?imageId={0}";

        /// <summary>
        /// HTTP request URL to get <see cref="Domain.Entities.PostImage"/> as file <br/>
        /// Use <see cref="string.Format(string, object?[])"/> <br/>
        /// {0} - <see cref="PostImage.Id"/>
        /// </summary>
        public const string PostImageRequest = "/PostImages/GetPostImage?imageId={0}";

        /// <summary>
        /// Angular client registration page URL <br/>
        /// Use <see cref="string.Format(string, object?[])"/> <br/>
        /// {0} - Angular client base URL <br/>
        /// {1} - Registration key
        /// </summary>
        public const string RegistrationUrl = "{0}/app/authentication/registration?registrationKey={1}";
    }
}