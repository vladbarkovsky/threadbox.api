using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;

namespace Seeding.Seeders
{
    public class PostSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public PostSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            Post[] posts = JsonSeed.Load<Post[]>(FilePathConstants.Json.Posts);

            _dbContext.Posts.AddRange(posts);
            await _dbContext.SaveChangesAsync();
        }
    }
}
