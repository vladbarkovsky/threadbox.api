namespace Seeding
{
    public static class FilePathConstants
    {
        public static class Json
        {
            public static readonly string Boards = Path.Combine("Data", "Json", "Boards.json");
            public static readonly string Threads = Path.Combine("Data", "Json", "Threads.json");
            public static readonly string Posts = Path.Combine("Data", "Json", "Posts.json");
        }

        public static readonly string CataasDirectory = Path.Combine("Data", "Cataas");
    }
}
