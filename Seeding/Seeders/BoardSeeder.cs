using Microsoft.EntityFrameworkCore;
using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;

namespace Seeding.Seeders
{
    public class BoardSeeder
    {
        private readonly ApplicationDbContext _dbContext;

        public BoardSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            Section section = await _dbContext.Sections.SingleAsync();
            Board[] boards = JsonSeed.Load<Board[]>(FilePathConstants.Json.Boards);

            foreach (Board board in boards)
            {
                board.SectionId = section.Id;
            }

            _dbContext.Boards.AddRange(boards);
            await _dbContext.SaveChangesAsync();
        }
    }
}
