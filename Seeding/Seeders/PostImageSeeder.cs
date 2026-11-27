using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Files;
using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web;

namespace Seeding.Seeders
{
    public class PostImageSeeder
    {
        private static readonly Guid FirstPostId = Guid.Parse("c8c22b70-4e32-486b-b161-d029cbc7b28c");
        private static readonly Guid SecondPostId = Guid.Parse("fe78e26e-b88c-4807-ada6-2f7ad376d14f");
        private static readonly Guid ThirdPostId = Guid.Parse("0bbbca15-836d-48be-8c3d-073996bf2982");
        private static readonly Guid FourthPostId = Guid.Parse("c830d4f1-f135-4861-9bbc-a89cf918d37e");
        private static readonly Guid FifthPostId = Guid.Parse("895b7d43-f8aa-42b6-8f5f-34de30e4e87b");
        private static readonly Guid SixthPostId = Guid.Parse("6591c57a-c172-4ca0-b5d4-43e81efbead0");
        private static readonly Guid SeventhPostId = Guid.Parse("34d0e6ba-06cd-4cc4-b61b-878950d6dd5b");
        private static readonly Guid EighthPostId = Guid.Parse("313c5f5f-d3c2-48cf-b363-207c908bb5b6");

        private readonly ApplicationDbContext _dbContext;
        private readonly IFileStorage _fileStorage;

        public PostImageSeeder(ApplicationDbContext dbContext, IFileStorage fileStorage)
        {
            _dbContext = dbContext;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            Dictionary<Guid, Post> posts = await _dbContext.Posts
                .ToDictionaryAsync(post => post.Id);

            await SeedAsync(posts[FirstPostId], "CataasImage11.jpeg");
            await SeedAsync(posts[SecondPostId], "CataasImage12.jpeg");
            await SeedAsync(posts[SecondPostId], "CataasImage13.jpeg");
            await SeedAsync(posts[ThirdPostId], "CataasImage14.jpeg");
            await SeedAsync(posts[ThirdPostId], "CataasImage15.jpeg");
            await SeedAsync(posts[ThirdPostId], "CataasImage16.png");
            await SeedAsync(posts[FourthPostId], "CataasImage17.png");
            await SeedAsync(posts[FourthPostId], "CataasImage18.jpeg");
            await SeedAsync(posts[FourthPostId], "CataasImage19.jpeg");
            await SeedAsync(posts[FourthPostId], "CataasImage20.jpeg");
            await SeedAsync(posts[FourthPostId], "CataasImage21.png");
            await SeedAsync(posts[FifthPostId], "CataasImage22.jpeg");
            await SeedAsync(posts[SixthPostId], "CataasImage23.jpeg");
            await SeedAsync(posts[SixthPostId], "CataasImage24.jpeg");
            await SeedAsync(posts[SeventhPostId], "CataasImage25.jpeg");
            await SeedAsync(posts[SeventhPostId], "CataasImage26.jpeg");
            await SeedAsync(posts[SeventhPostId], "CataasImage27.jpeg");
            await SeedAsync(posts[EighthPostId], "CataasImage28.jpeg");
            await SeedAsync(posts[EighthPostId], "CataasImage29.jpeg");
            await SeedAsync(posts[EighthPostId], "CataasImage30.jpeg");
            await SeedAsync(posts[EighthPostId], "CataasImage31.jpeg");
            await SeedAsync(posts[EighthPostId], "CataasImage32.jpeg");

            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedAsync(Post post, string fileName)
        {
            string storagePath = Path.Combine("PostImages", $"Post_{post.Id}", fileName);

            post.PostImages.Add(new PostImage
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
