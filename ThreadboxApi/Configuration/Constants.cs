namespace ThreadboxApi.Configuration
{
	public class Constants
	{
		public const string TestImagesDirectory = "CataasImages";
		public const string SeedingDataPath = @"Configuration/Seeding";

		public const string BoardsSeedingFilePath = $"{SeedingDataPath}/boards.json";
		public const string ThreadsSeedingFilePath = $"{SeedingDataPath}/threads.json";
		public const string PostsSeedingFilePath = $"{SeedingDataPath}/posts.json";

		/// <summary>
		/// HTTP request URL to get <see cref="Models.ThreadImage"/> as file.
		/// Use <see cref="string.Format(string, object?[])"/>.
		/// {0} - <see cref="Models.ThreadImage.Id"/>
		/// </summary>
		public const string ThreadImageRequest = "/ThreadImages/GetThreadImage?imageId={0}";

		/// <summary>
		/// HTTP request URL to get <see cref="Models.PostImage"/> as file.
		/// Use <see cref="string.Format(string, object?[])"/>.
		/// {0} - <see cref="Models.PostImage.Id"/>
		/// </summary>
		public const string PostImageRequest = "/PostImages/GetPostImage?imageId={0}";
	}
}