using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;

namespace Seeding.Seeders
{
    public class SectionSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public SectionSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            var section = new Section
            {
                Title = "Web programming"
            };

            _dbContext.Sections.Add(section);
            await _dbContext.SaveChangesAsync();
        }
    }
}
