namespace ThreadboxApi.ORM.Seeding
{
    public class SeedingConstants
    {
        public const string CataasDirectory = @$"{SeedDirectory}\Cataas";

        public class JsonFiles
        {
            public const string Boards = @$"{JsonDirectory}\Boards.json";
            public const string Threads = @$"{JsonDirectory}\Threads.json";
            public const string Posts = @$"{JsonDirectory}\Posts.json";
        }

        private const string SeedDirectory = @"Infrastructure\Persistence\Seeding";
        private const string JsonDirectory = @$"{SeedDirectory}\Json";
    }
}