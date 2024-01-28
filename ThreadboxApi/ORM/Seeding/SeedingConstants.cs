namespace ThreadboxApi.ORM.Seeding
{
    public class SeedingConstants
    {
        private const string SeedDirectory = @"ORM\Seeding";
        private const string JsonDirectory = @$"{SeedDirectory}\Json";

        public class JsonFiles
        {
            public const string Boards = @$"{JsonDirectory}\Boards.json";
            public const string Threads = @$"{JsonDirectory}\Threads.json";
            public const string Posts = @$"{JsonDirectory}\Posts.json";
        }

        public const string CataasDirectory = @$"{SeedDirectory}\Cataas";
    }
}