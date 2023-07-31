namespace ThreadboxApi.Infrastructure.Persistence.Seeding
{
    public class SeedingConstants
    {
        public const string SeedDirectory = @"Infrastructure\Persistence\Seeding";

        public const string JsonDirectory = @$"{SeedDirectory}\JSON";
        public const string CataasImagesDirectory = @$"{SeedDirectory}\CataasImages";

        public const string BoardsSeedFile = @$"{JsonDirectory}\Boards.json";
        public const string ThreadsSeedFile = @$"{JsonDirectory}\Threads.json";
        public const string PostsSeedFile = @$"{JsonDirectory}\Posts.json";
    }
}