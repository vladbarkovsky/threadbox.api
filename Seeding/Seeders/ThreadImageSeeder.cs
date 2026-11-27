using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Files;
using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web;

namespace Seeding.Seeders
{
    public class ThreadImageSeeder
    {
        private static readonly Guid FirstThreadId = Guid.Parse("2c891524-8da2-4e58-b5a3-82fc1b93994e");
        private static readonly Guid SecondThreadId = Guid.Parse("234aa430-e4f4-47a1-99d1-51df1079fd11");
        private static readonly Guid ThirdThreadId = Guid.Parse("1908bc11-b100-467e-9dae-b088bd0e3596");
        private static readonly Guid FourthThreadId = Guid.Parse("d9517f55-3e98-4c94-8e39-2bee212e0367");

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public ThreadImageSeeder(ApplicationDbContext dbContext, IFileStorage fileStorage)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            Dictionary<Guid, ThreadboxApi.ORM.Entities.Thread> threads = await _dbContext.Threads
                .ToDictionaryAsync(thread => thread.Id);

            await SeedAsync(threads[FirstThreadId], "CataasImage0.png");
            await SeedAsync(threads[SecondThreadId], "CataasImage1.jpeg");
            await SeedAsync(threads[SecondThreadId], "CataasImage2.png");
            await SeedAsync(threads[ThirdThreadId], "CataasImage3.jpeg");
            await SeedAsync(threads[ThirdThreadId], "CataasImage4.jpeg");
            await SeedAsync(threads[ThirdThreadId], "CataasImage5.jpeg");
            await SeedAsync(threads[FourthThreadId], "CataasImage6.jpeg");
            await SeedAsync(threads[FourthThreadId], "CataasImage7.png");
            await SeedAsync(threads[FourthThreadId], "CataasImage8.jpeg");
            await SeedAsync(threads[FourthThreadId], "CataasImage9.jpeg");
            await SeedAsync(threads[FourthThreadId], "CataasImage10.jpeg");
        }

        private async Task SeedAsync(ThreadboxApi.ORM.Entities.Thread thread, string fileName)
        {
            Guid threadImageId = Guid.NewGuid();
            string storagePath = Path.Combine("ThreadImages", $"Thread_{thread.Id}", threadImageId.ToString());

            thread.ThreadImages.Add(new ThreadImage
            {
                FileInfo = new ThreadboxApi.ORM.Entities.FileInfo
                {
                    Name = fileName,
                    ContentType = ContentType.Get(fileName),
                    Path = storagePath
                }
            });

            byte[] file = await File.ReadAllBytesAsync(Path.Combine(FilePathConstants.CataasDirectory, fileName));
            await _fileStorage.SaveFileAsync(storagePath, file);
        }
    }
}
