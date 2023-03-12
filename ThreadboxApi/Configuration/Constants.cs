namespace ThreadboxApi.Configuration
{
	public class Constants
	{
		public const string SeedingImagesPath = @"CataasImages";
		private const string SeedingDataPath = @"Configuration\Seeding";

		public const string BoardsSeedingFilePath = @$"{SeedingDataPath}\boards.json";
		public const string ThreadsSeedingFilePath = @$"{SeedingDataPath}\threads.json";
		public const string PostsSeedingFilePath = @$"{SeedingDataPath}\posts.json";

		/// <summary>
		/// HTTP request URL to get <see cref="Models.ThreadImage"/> as file <br/>
		/// Use <see cref="string.Format(string, object?[])"/> <br/>
		/// {0} - <see cref="Models.ThreadImage.Id"/>
		/// </summary>
		public const string ThreadImageRequest = "/ThreadImages/GetThreadImage?imageId={0}";

		/// <summary>
		/// HTTP request URL to get <see cref="Models.PostImage"/> as file <br/>
		/// Use <see cref="string.Format(string, object?[])"/> <br/>
		/// {0} - <see cref="Models.PostImage.Id"/>
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