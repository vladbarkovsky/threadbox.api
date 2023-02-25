namespace ThreadboxApi.Configuration
{
	public class Constants
	{
		public const string TestImagesDirectory = "CataasImages";

		/// <summary>
		/// HTTP request URL to get <see cref="Models.ThreadImage"/> as file.
		/// Use <see cref="string.Format(string, object?[])"/>.
		/// {0} - <see cref="Models.ThreadImage.Id"/>
		/// </summary>
		public const string ThreadImageRequest = "/Images/GetThreadImage?imageId={0}";

		/// <summary>
		/// HTTP request URL to get <see cref="Models.PostImage"/> as file.
		/// Use <see cref="string.Format(string, object?[])"/>.
		/// {0} - <see cref="Models.PostImage.Id"/>
		/// </summary>
		public const string PostImageRequest = "/Images/GetPostImage?imageId={0}";
	}
}