namespace ThreadboxApi.Infrastructure.Persistence.Seed
{
    public class Constants
    {
        public const string SeedDirectory = @"Infrastructure\Persistence\Seed";

        public const string JsonDirectory = @$"{SeedDirectory}\JSON";
        public const string CataasImagesDirectory = @$"{SeedDirectory}\CataasImages";

        public const string BoardsSeedingFile = @$"{JsonDirectory}\Boards.json";
        public const string ThreadsSeedingFile = @$"{JsonDirectory}\Threads.json";
        public const string PostsSeedingFile = @$"{JsonDirectory}\Posts.json";
    }
}