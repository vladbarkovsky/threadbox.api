using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Identity.Roles;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace Seeding
{
    public class SeedingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorage _fileStorage;
        private readonly RoleManager<IdentityRole> _roleManager;

        private JsonSerializerOptions JsonSerializerOptions { get; }

        public SeedingService(
            ApplicationDbContext dbContext,
            IOptionsSnapshot<AppSettings> appSettings,
            UserManager<ApplicationUser> userManager,
            IFileStorage fileStorage,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _appSettings = appSettings;
            _userManager = userManager;
            _fileStorage = fileStorage;
            _roleManager = roleManager;

            JsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task SeedAsync()
        {
            await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await SeedRolesAsync();
                await SeedUsersAsync();
                await SeedSections();
                await SeedBoardsAsync();
                await SeedThreadsAsync();
                await SeedThreadImagesAsync();
                await SeedPosts();
                await SeedPostImagesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            IEnumerable<Type> roleTypes = Reflection.GetRoleTypes();

            foreach (Type roleType in roleTypes)
            {
                string roleName = Reflection.GetRoleName(roleType);
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                IdentityRole role = await _roleManager.FindByNameAsync(roleName);

                IEnumerable<string> rolePermissions = Reflection.GetRolePermissions(roleType);

                foreach (string permission in rolePermissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(PermissionConstants.ClaimType, permission));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            var admin = new ApplicationUser
            {
                UserName = _appSettings.Value.DefaultAdminCredentials.Username
            };

            await _userManager.CreateAsync(admin, _appSettings.Value.DefaultAdminCredentials.Password);
            await _userManager.AddToRoleAsync(admin, AdminRole.Name);

            var manager = new ApplicationUser
            {
                UserName = "manager"
            };

            await _userManager.CreateAsync(manager, _appSettings.Value.DefaultAdminCredentials.Password);
            await _userManager.AddToRoleAsync(manager, ManagerRole.Name);
        }

        private async Task SeedSections()
        {
            var section = new Section
            {
                Title = "Web programming"
            };

            _dbContext.Sections.Add(section);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedBoardsAsync()
        {
            Section section = await _dbContext.Sections.SingleAsync();
            Board[] boards = LoadFromJson<Board[]>(FilePathConstants.Json.Boards);

            foreach (Board board in boards)
            {
                board.SectionId = section.Id;
            }

            _dbContext.Boards.AddRange(boards);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedThreadsAsync()
        {
            Board board = await _dbContext.Boards
                .Where(bool (Board board) => board.Id == Guid.Parse("4cd02ec6-de02-45f3-94f3-108a0c139892"))
                .SingleAsync();

            ThreadboxApi.ORM.Entities.Thread[] threads = LoadFromJson<ThreadboxApi.ORM.Entities.Thread[]>(
                FilePathConstants.Json.Threads);

            foreach (ThreadboxApi.ORM.Entities.Thread thread in threads)
            {
                thread.BoardId = board.Id;
            }

            _dbContext.Threads.AddRange(threads);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedThreadImagesAsync()
        {
            List<ThreadboxApi.ORM.Entities.Thread> threads = await _dbContext.Threads.ToListAsync();

            await SeedThreadImageAsync(threads[0], "CataasImage0.png");
            await SeedThreadImageAsync(threads[1], "CataasImage1.jpeg");
            await SeedThreadImageAsync(threads[1], "CataasImage2.png");
            await SeedThreadImageAsync(threads[2], "CataasImage3.jpeg");
            await SeedThreadImageAsync(threads[2], "CataasImage4.jpeg");
            await SeedThreadImageAsync(threads[2], "CataasImage5.jpeg");
            await SeedThreadImageAsync(threads[3], "CataasImage6.jpeg");
            await SeedThreadImageAsync(threads[3], "CataasImage7.png");
            await SeedThreadImageAsync(threads[3], "CataasImage8.jpeg");
            await SeedThreadImageAsync(threads[3], "CataasImage9.jpeg");
            await SeedThreadImageAsync(threads[3], "CataasImage10.jpeg");
        }

        private async Task SeedPosts()
        {
            List<ThreadboxApi.ORM.Entities.Thread> threads = await _dbContext.Threads.ToListAsync();
            Post[] posts = LoadFromJson<Post[]>(FilePathConstants.Json.Posts);

            posts[0].ThreadId = threads[0].Id;

            posts[1].ThreadId = threads[1].Id;
            posts[2].ThreadId = threads[1].Id;

            posts[3].ThreadId = threads[2].Id;
            posts[4].ThreadId = threads[2].Id;
            posts[5].ThreadId = threads[2].Id;

            posts[6].ThreadId = threads[3].Id;
            posts[7].ThreadId = threads[3].Id;
            posts[8].ThreadId = threads[3].Id;
            posts[9].ThreadId = threads[3].Id;
            posts[10].ThreadId = threads[3].Id;

            _dbContext.Posts.AddRange(posts);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedPostImagesAsync()
        {
            List<Post> posts = _dbContext.Posts.Local.ToList();

            await SeedPostImageAsync(posts[0], "CataasImage11.jpeg");
            await SeedPostImageAsync(posts[1], "CataasImage12.jpeg");
            await SeedPostImageAsync(posts[1], "CataasImage13.jpeg");
            await SeedPostImageAsync(posts[2], "CataasImage14.jpeg");
            await SeedPostImageAsync(posts[2], "CataasImage15.jpeg");
            await SeedPostImageAsync(posts[2], "CataasImage16.png");
            await SeedPostImageAsync(posts[3], "CataasImage17.png");
            await SeedPostImageAsync(posts[3], "CataasImage18.jpeg");
            await SeedPostImageAsync(posts[3], "CataasImage19.jpeg");
            await SeedPostImageAsync(posts[3], "CataasImage20.jpeg");
            await SeedPostImageAsync(posts[3], "CataasImage21.png");
            await SeedPostImageAsync(posts[4], "CataasImage22.jpeg");
            await SeedPostImageAsync(posts[5], "CataasImage23.jpeg");
            await SeedPostImageAsync(posts[5], "CataasImage24.jpeg");
            await SeedPostImageAsync(posts[6], "CataasImage25.jpeg");
            await SeedPostImageAsync(posts[6], "CataasImage26.jpeg");
            await SeedPostImageAsync(posts[6], "CataasImage27.jpeg");
            await SeedPostImageAsync(posts[7], "CataasImage28.jpeg");
            await SeedPostImageAsync(posts[7], "CataasImage29.jpeg");
            await SeedPostImageAsync(posts[7], "CataasImage30.jpeg");
            await SeedPostImageAsync(posts[7], "CataasImage31.jpeg");
            await SeedPostImageAsync(posts[7], "CataasImage32.jpeg");

            await _dbContext.SaveChangesAsync();
        }

        private T LoadFromJson<T>(string path)
        {
            string data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);
        }

        private async Task SeedThreadImageAsync(ThreadboxApi.ORM.Entities.Thread thread, string fileName)
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

        private async Task SeedPostImageAsync(Post post, string fileName)
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
