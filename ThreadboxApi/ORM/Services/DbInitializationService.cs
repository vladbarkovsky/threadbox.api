using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Identity.Roles;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Seeding;

namespace ThreadboxApi.ORM.Services
{
    public class DbInitializationService : ITransientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorage _fileStorage;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleSynchronizer _roleSynchronizer;

        private JsonSerializerOptions JsonSerializerOptions { get; }

        public DbInitializationService(
            ApplicationDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IFileStorage fileStorage,
            RoleManager<IdentityRole> roleManager,
            RoleSynchronizer roleSynchronizer)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _userManager = userManager;
            _fileStorage = fileStorage;
            _roleManager = roleManager;
            _roleSynchronizer = roleSynchronizer;

            JsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task EnsureInitializedAsync()
        {
            // NOTE: NSwag target in project file does something with reflection, and application code
            // executes during build because of NSwag target and again during application launch.
            // Therefore don't be surprised that databaseExists is true.
            var databaseExists = await _dbContext.Database.CanConnectAsync();

            if (!databaseExists)
            {
                _dbContext.Database.Migrate();
                await SeedAsync();
                await _dbContext.SaveChangesAsync();
                Log.Information("Database initialized and seeded.");
            }
            else
            {
                await _roleSynchronizer.SyncAsync();
                await _dbContext.SaveChangesAsync();
                Log.Information("Roles synchronized.");
            }
        }

        private async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();

            if (!_webHostEnvironment.IsDevelopment())
            {
                return;
            }

            SeedSections();
            SeedBoards();
            SeedThreads();
            await SeedThreadImagesAsync();
            SeedPosts();
            await SeedPostImagesAsync();
        }

        public async Task SeedRolesAsync()
        {
            var roleTypes = Reflection.GetRoleTypes();

            foreach (var roleType in roleTypes)
            {
                var roleName = Reflection.GetRoleName(roleType);
                await _roleManager.CreateAsync(new IdentityRole(roleName));
                var role = await _roleManager.FindByNameAsync(roleName);

                var rolePermissions = Reflection.GetRolePermissions(roleType);

                foreach (var permission in rolePermissions)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(PermissionContants.ClaimType, permission));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            var admin = new ApplicationUser
            {
                UserName = _configuration[AppSettings.DefaultAdminCredentials.UserName]
            };

            await _userManager.CreateAsync(admin, _configuration[AppSettings.DefaultAdminCredentials.Password]);
            await _userManager.AddToRoleAsync(admin, AdminRole.Name);

            if (!_webHostEnvironment.IsDevelopment())
            {
                return;
            }

            var manager = new ApplicationUser
            {
                UserName = "manager"
            };

            await _userManager.CreateAsync(manager, _configuration[AppSettings.DefaultAdminCredentials.Password]);
            await _userManager.AddToRoleAsync(manager, ManagerRole.Name);
        }

        private void SeedSections()
        {
            var section = new Section
            {
                Title = "Web programming"
            };

            _dbContext.Sections.Add(section);
        }

        private void SeedBoards()
        {
            var section = _dbContext.Sections.Local.First();
            section.Boards = LoadFromJson<List<Board>>(SeedingConstants.JsonFiles.Boards);
        }

        private void SeedThreads()
        {
            var board = _dbContext.Boards.Local.First();
            board.Threads = LoadFromJson<List<Entities.Thread>>(SeedingConstants.JsonFiles.Threads);
        }

        private async Task SeedThreadImagesAsync()
        {
            var threads = _dbContext.Threads.Local.ToList();

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

        private void SeedPosts()
        {
            var threads = _dbContext.Threads.Local.ToList();
            var posts = LoadFromJson<List<Post>>(SeedingConstants.JsonFiles.Posts);

            threads[0].Posts = posts.GetRange(0, 1);
            threads[1].Posts = posts.GetRange(1, 2);
            threads[2].Posts = posts.GetRange(3, 3);
            threads[3].Posts = posts.GetRange(6, 5);
        }

        private async Task SeedPostImagesAsync()
        {
            var posts = _dbContext.Posts.Local.ToList();

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
        }

        private T LoadFromJson<T>(string path)
        {
            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);
        }

        // NOTE: Distributed transaction unsafe.
        private async Task SeedThreadImageAsync(Entities.Thread thread, string fileName)
        {
            var threadImageId = Guid.NewGuid();
            var storagePath = @$"ThreadImages\Thread_{thread.Id}\{threadImageId}";

            thread.ThreadImages.Add(new ThreadImage
            {
                FileInfo = new Entities.FileInfo
                {
                    Name = fileName,
                    ContentType = ContentType.Get(fileName),
                    Path = storagePath
                }
            });

            var file = await File.ReadAllBytesAsync(@$"{SeedingConstants.CataasDirectory}\{fileName}");
            await _fileStorage.SaveFileAsync(storagePath, file);
        }

        // NOTE: Distributed transaction unsafe.
        private async Task SeedPostImageAsync(Post post, string fileName)
        {
            var storagePath = @$"PostImages\Post_{post.Id}\{fileName}";

            post.PostImages.Add(new PostImage
            {
                FileInfo = new Entities.FileInfo
                {
                    Name = fileName,
                    ContentType = ContentType.Get(fileName),
                    Path = storagePath
                }
            });

            var file = await File.ReadAllBytesAsync(@$"{SeedingConstants.CataasDirectory}\{fileName}");
            await _fileStorage.SaveFileAsync(storagePath, file);
        }
    }
}