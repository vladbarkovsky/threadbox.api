using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Helpers.Utilities;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Application.Identity;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Infrastructure.Persistence.Seeding
{
    public class DbInitializationService : ITransientService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorage _fileStorage;
        private readonly RoleManager<IdentityRole> _roleManager;

        private JsonSerializerOptions JsonSerializerOptions { get; }

        public DbInitializationService(
            ApplicationDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IFileStorage fileStorage,
            RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
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

        // NOTE: NSwag target in project file does something with reflection, and application code
        // executes during build because of NSwag target and again during application launch.
        // Therefore don't be surprised that databaseExists == true
        public async Task EnsureInitializedAsync()
        {
            var databaseExists = await _appDbContext.Database.CanConnectAsync();

            if (!databaseExists)
            {
                _appDbContext.Database.Migrate();
                await SeedAsync();
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
            var adminPermissions = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.IsAssignableTo(typeof(IPermissionSet)) && x.IsClass)
                .SelectMany(x => x.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                .Select(x => x.GetRawConstantValue().ToString());

            var adminRole = await _roleManager.FindByNameAsync(Roles.Admin);

            foreach (var permission in adminPermissions)
            {
                await _roleManager.AddClaimAsync(adminRole, new Claim(PermissionContants.ClaimType, permission));
            }

            var moderatorPermissions = new List<string>
            {
                // Default permissions for moderator
            };

            var moderatorRole = await _roleManager.FindByNameAsync(Roles.Moderator);

            foreach (var permission in moderatorPermissions)
            {
                await _roleManager.AddClaimAsync(moderatorRole, new Claim(PermissionContants.ClaimType, permission));
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

            var moderator = new ApplicationUser
            {
                UserName = "moderator"
            };

            await _userManager.CreateAsync(moderator, _configuration[AppSettings.DefaultAdminCredentials.Password]);
            await _userManager.AddToRoleAsync(moderator, Roles.Moderator);
        }

        private async Task SeedBoardsAsync()
        {
            var boards = LoadFromJson<List<Board>>(SeedingConstants.BoardsSeedFile);
            await _appDbContext.AddRangeAsync(boards);
            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedThreadsAsync()
        {
            var boards = await _appDbContext.Boards.ToListAsync();
            var threads = LoadFromJson<List<Domain.Entities.Thread>>(SeedingConstants.ThreadsSeedFile);
            boards.First().Threads = threads;
            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedThreadImagesAsync()
        {
            var threads = await _appDbContext.Threads.ToListAsync();

            await SeedThreadImageAsync(threads[0], "CataasImage0.");
            await SeedThreadImageAsync(threads[1], "CataasImage1.");
            await SeedThreadImageAsync(threads[1], "CataasImage2.");
            await SeedThreadImageAsync(threads[2], "CataasImage3.");
            await SeedThreadImageAsync(threads[2], "CataasImage4.");
            await SeedThreadImageAsync(threads[2], "CataasImage5.");
            await SeedThreadImageAsync(threads[3], "CataasImage6.");
            await SeedThreadImageAsync(threads[3], "CataasImage7.");
            await SeedThreadImageAsync(threads[3], "CataasImage8.");
            await SeedThreadImageAsync(threads[3], "CataasImage9.");
            await SeedThreadImageAsync(threads[3], "CataasImage10.");

            _appDbContext.Threads.UpdateRange(threads);
            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedPostsAsync()
        {
            var threads = await _appDbContext.Threads.ToListAsync();
            var posts = LoadFromJson<List<Post>>(SeedingConstants.PostsSeedFile);

            threads[0].Posts = posts.GetRange(0, 1);
            threads[1].Posts = posts.GetRange(1, 2);
            threads[2].Posts = posts.GetRange(3, 3);
            threads[3].Posts = posts.GetRange(6, 5);

            _appDbContext.Threads.UpdateRange(threads);
            await _appDbContext.Posts.AddRangeAsync(posts);
            await _appDbContext.SaveChangesAsync();
        }

        private async Task SeedPostImagesAsync()
        {
            var posts = await _appDbContext.Posts.ToListAsync();

            await SeedPostImageAsync(posts[0], "CataasImage11.");
            await SeedPostImageAsync(posts[1], "CataasImage12.");
            await SeedPostImageAsync(posts[1], "CataasImage13.");
            await SeedPostImageAsync(posts[2], "CataasImage14.");
            await SeedPostImageAsync(posts[2], "CataasImage15.");
            await SeedPostImageAsync(posts[2], "CataasImage16.");
            await SeedPostImageAsync(posts[3], "CataasImage17.");
            await SeedPostImageAsync(posts[3], "CataasImage18.");
            await SeedPostImageAsync(posts[3], "CataasImage19.");
            await SeedPostImageAsync(posts[3], "CataasImage20.");
            await SeedPostImageAsync(posts[3], "CataasImage21.");
            await SeedPostImageAsync(posts[4], "CataasImage22.");
            await SeedPostImageAsync(posts[5], "CataasImage23.");
            await SeedPostImageAsync(posts[5], "CataasImage24.");
            await SeedPostImageAsync(posts[6], "CataasImage25.");
            await SeedPostImageAsync(posts[6], "CataasImage26.");
            await SeedPostImageAsync(posts[6], "CataasImage27.");
            await SeedPostImageAsync(posts[7], "CataasImage28.");
            await SeedPostImageAsync(posts[7], "CataasImage29.");
            await SeedPostImageAsync(posts[7], "CataasImage30.");
            await SeedPostImageAsync(posts[7], "CataasImage31.");
            await SeedPostImageAsync(posts[7], "CataasImage32.");

            _appDbContext.Posts.UpdateRange(posts);
            await _appDbContext.SaveChangesAsync();
        }

        private T LoadFromJson<T>(string path)
        {
            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);
        }

        private async Task SeedThreadImageAsync(Domain.Entities.Thread thread, string fileName)
        {
            var threadImageId = Guid.NewGuid();
            var storagePath = @$"ThreadImages\Thread_{thread.Id}\{threadImageId}";

            thread.ThreadImages.Add(new ThreadImage
            {
                FileInfo = new Domain.Entities.FileInfo
                {
                    Name = fileName,
                    ContentType = ContentType.Get(fileName),
                    Path = storagePath
                }
            });

            var file = await File.ReadAllBytesAsync(@$"{SeedingConstants.CataasDirectory}\{fileName}");
            await _fileStorage.SaveFileAsync(storagePath, file);
        }

        private async Task SeedPostImageAsync(Post post, string fileName)
        {
            var storagePath = @$"PostImages\Post{post.Id}\{fileName}";

            post.PostImages.Add(new PostImage
            {
                FileInfo = new Domain.Entities.FileInfo
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