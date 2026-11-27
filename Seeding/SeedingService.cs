using Microsoft.EntityFrameworkCore.Storage;
using Seeding.Seeders;
using ThreadboxApi.ORM;

namespace Seeding
{
    public class SeedingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly OpenIddictApplicationSeeder _openIddictApplicationSeeder;
        private readonly RoleSeeder _roleSeeder;
        private readonly UserSeeder _userSeeder;
        private readonly SectionSeeder _sectionSeeder;
        private readonly BoardSeeder _boardSeeder;
        private readonly ThreadSeeder _threadSeeder;
        private readonly ThreadImageSeeder _threadImageSeeder;
        private readonly PostSeeder _postSeeder;
        private readonly PostImageSeeder _postImageSeeder;

        public SeedingService(
            ApplicationDbContext dbContext,
            OpenIddictApplicationSeeder openIddictApplicationSeeder,
            RoleSeeder roleSeeder,
            UserSeeder userSeeder,
            SectionSeeder sectionSeeder,
            BoardSeeder boardSeeder,
            ThreadSeeder threadSeeder,
            ThreadImageSeeder threadImageSeeder,
            PostSeeder postSeeder,
            PostImageSeeder postImageSeeder)
        {
            _dbContext = dbContext;
            _openIddictApplicationSeeder = openIddictApplicationSeeder;
            _roleSeeder = roleSeeder;
            _userSeeder = userSeeder;
            _sectionSeeder = sectionSeeder;
            _boardSeeder = boardSeeder;
            _threadSeeder = threadSeeder;
            _threadImageSeeder = threadImageSeeder;
            _postSeeder = postSeeder;
            _postImageSeeder = postImageSeeder;
        }

        public async Task SeedAsync()
        {
            await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await _openIddictApplicationSeeder.SeedAsync();
                await _roleSeeder.SeedAsync();
                await _userSeeder.SeedAsync();
                await _sectionSeeder.SeedAsync();
                await _boardSeeder.SeedAsync();
                await _threadSeeder.SeedAsync();
                await _threadImageSeeder.SeedAsync();
                await _postSeeder.SeedAsync();
                await _postImageSeeder.SeedAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
