﻿using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Application.Files.Interfaces;
using ThreadboxApi.Application.Identity;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Infrastructure.Persistence.Seeding
{
    public class DbInitializationService : ITransientService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileStorage _fileStorage;
        private readonly RoleManager<IdentityRole> _roleManager;

        private JsonSerializerOptions JsonSerializerOptions { get; }

        public DbInitializationService(
            ApplicationDbContext appDbContext,
            PersistedGrantDbContext persistentGrantDbContext,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IFileStorage fileStorage,
            RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _persistedGrantDbContext = persistentGrantDbContext;
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
                _persistedGrantDbContext.Database.Migrate();
                await SeedAsync();
            }
        }

        private async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();

            if (_webHostEnvironment.IsDevelopment())
            {
                await SeedBoardsAsync();
                await SeedThreadsAsync();
                await SeedThreadImagesAsync();
                await SeedPostsAsync();
                await SeedPostImagesAsync();
            }

            System.Diagnostics.Debug.WriteLine("Database initialized and seeded.");
        }

        public async Task SeedRolesAsync()
        {
            var typeof(Roles).GetFields().Select(x => x.GetRawConstantValue());

            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    object value = field.GetValue(null);
                    Console.WriteLine($"Имя константы: {field.Name}, Значение: {value}");
                }
            }

            await _roleManager.AddClaimAsync()
        }

        private async Task SeedUsersAsync()
        {
            var admin = new ApplicationUser
            {
                UserName = _configuration[AppSettings.DefaultAdminCredentials.UserName]
            };

            await _userManager.CreateAsync(admin, _configuration[AppSettings.DefaultAdminCredentials.Password]);
            await _userManager.AddToRoleAsync(admin, "Admin");

            if (_webHostEnvironment.IsProduction())
            {
                return;
            }

            // Seed other users for development purposes
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

            //threads[0].ThreadImages = await _fileSeedingService.CreateFiles<ThreadImage>(1);
            //threads[1].ThreadImages = await _fileSeedingService.CreateFiles<ThreadImage>(2);
            //threads[2].ThreadImages = await _fileSeedingService.CreateFiles<ThreadImage>(3);
            //threads[3].ThreadImages = await _fileSeedingService.CreateFiles<ThreadImage>(5);

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

            //posts[0].PostImages = await _fileSeedingService.CreateFiles<PostImage>(1);
            //posts[1].PostImages = await _fileSeedingService.CreateFiles<PostImage>(2);
            //posts[2].PostImages = await _fileSeedingService.CreateFiles<PostImage>(3);
            //posts[3].PostImages = await _fileSeedingService.CreateFiles<PostImage>(5);
            //posts[4].PostImages = await _fileSeedingService.CreateFiles<PostImage>(1);
            //posts[5].PostImages = await _fileSeedingService.CreateFiles<PostImage>(2);
            //posts[6].PostImages = await _fileSeedingService.CreateFiles<PostImage>(3);
            //posts[7].PostImages = await _fileSeedingService.CreateFiles<PostImage>(5);

            _appDbContext.Posts.UpdateRange(posts);
            await _appDbContext.SaveChangesAsync();
        }

        private T LoadFromJson<T>(string path)
        {
            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions);
        }
    }
}