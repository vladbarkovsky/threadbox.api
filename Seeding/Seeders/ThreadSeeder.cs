using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;

namespace Seeding.Seeders
{
    public class ThreadSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public ThreadSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            ThreadboxApi.ORM.Entities.Thread[] threads = JsonSeed.Load<ThreadboxApi.ORM.Entities.Thread[]>(FilePathConstants.Json.Threads);

            _dbContext.Threads.AddRange(threads);
            await _dbContext.SaveChangesAsync();
        }
    }
}
