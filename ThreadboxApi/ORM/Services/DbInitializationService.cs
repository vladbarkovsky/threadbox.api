using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Identity;
using ThreadboxApi.Application.Identity.Permissions;
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

        private JsonSerializerOptions JsonSerializerOptions { get; }

        public DbInitializationService(
            ApplicationDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IFileStorage fileStorage,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _userManager = userManager;
            _fileStorage = fileStorage;
            _roleManager = roleManager;

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
            // Therefore don't be surprised that databaseExists is true
            var databaseExists = await _dbContext.Database.CanConnectAsync();

            if (!databaseExists)
            {
                _dbContext.Database.Migrate();
                await SeedAsync();
            }
            else
            {
                // TODO: Update permissions
            }
        }

        private async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedPermissionsAsync();
            await SeedUsersAsync();

            if (_webHostEnvironment.IsDevelopment())
            {
                await SeedBoardsAsync();
                await SeedThreadsAsync();
                await SeedThreadImagesAsync();
                await SeedPostsAsync();
                await SeedPostImagesAsync();
            }

            Log.Information("Database initialized and seeded.");
        }

        public async Task SeedRolesAsync()
        {
            var roleConstants = typeof(Roles).GetFields();

            foreach (var constant in roleConstants)
            {
                await _roleManager.CreateAsync(new IdentityRole(constant.GetRawConstantValue().ToString()));
            }
        }

        public async Task SeedPermissionsAsync()
        {
            var allPermissions = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsAssignableTo(typeof(IPermissionSet)) && x.IsClass)
                .SelectMany(x => x.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                .Select(x => x.GetRawConstantValue().ToString());

            var adminRole = await _roleManager.FindByNameAsync(Roles.Admin);

            foreach (var permission in allPermissions)
            {
                await _roleManager.AddClaimAsync(adminRole, new Claim(PermissionContants.ClaimType, permission));
            }

            var managerPermissions = new List<string>
            {
                // Default permissions for manager
            };

            var managerRole = await _roleManager.FindByNameAsync(Roles.Manager);

            foreach (var permission in managerPermissions)
            {
                await _roleManager.AddClaimAsync(managerRole, new Claim(PermissionContants.ClaimType, permission));
            }
        }

        private async Task SeedUsersAsync()
        {
            var admin = new ApplicationUser
            {
                UserName = _configuration[AppSettings.DefaultAdminCredentials.UserName]
            };

            await _userManager.CreateAsync(admin, _configuration[AppSettings.DefaultAdminCredentials.Password]);
            await _userManager.AddToRoleAsync(admin, Roles.Admin);

            if (!_webHostEnvironment.IsDevelopment())
            {
                return;
            }

            var manager = new ApplicationUser
            {
                UserName = "manager"
            };

            await _userManager.CreateAsync(manager, _configuration[AppSettings.DefaultAdminCredentials.Password]);
            await _userManager.AddToRoleAsync(manager, Roles.Manager);
        }

        private async Task SeedBoardsAsync()
        {
            var boards = LoadFromJson<List<Board>>(SeedingConstants.JsonFiles.Boards);
            _dbContext.AddRange(boards);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedThreadsAsync()
        {
            var boards = await _dbContext.Boards.ToListAsync();
            var threads = LoadFromJson<List<Entities.Thread>>(SeedingConstants.JsonFiles.Threads);
            boards.First().Threads = threads;
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedThreadImagesAsync()
        {
            var threads = await _dbContext.Threads.ToListAsync();

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

            _dbContext.Threads.UpdateRange(threads);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedPostsAsync()
        {
            var threads = await _dbContext.Threads.ToListAsync();
            var posts = LoadFromJson<List<Post>>(SeedingConstants.JsonFiles.Posts);

            threads[0].Posts = posts.GetRange(0, 1);
            threads[1].Posts = posts.GetRange(1, 2);
            threads[2].Posts = posts.GetRange(3, 3);
            threads[3].Posts = posts.GetRange(6, 5);

            _dbContext.Threads.UpdateRange(threads);
            await _dbContext.Posts.AddRangeAsync(posts);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedPostImagesAsync()
        {
            var posts = await _dbContext.Posts.ToListAsync();

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

            _dbContext.Posts.UpdateRange(posts);
            await _dbContext.SaveChangesAsync();
        }

        private T LoadFromJson<T>(string path)
        {
            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);
        }

        // FIXME: Distributed transaction unsafe.
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

        // FIXME: Distributed transaction unsafe.
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