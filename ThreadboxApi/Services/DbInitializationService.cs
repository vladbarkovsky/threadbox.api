using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class DbInitializationService : ITransientService
	{
		private readonly ThreadboxDbContext _dbContext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _configuration;
		private readonly UserManager<User> _userManager;
		private readonly FileSeedingService _fileSeedingService;

		private JsonSerializerOptions JsonSerializerOptions { get; }

		public DbInitializationService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();
			_configuration = services.GetRequiredService<IConfiguration>();
			_userManager = services.GetRequiredService<UserManager<User>>();
			_fileSeedingService = services.GetRequiredService<FileSeedingService>();

			JsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true,
				Converters = { new JsonStringEnumConverter() }
			};
		}

		public async Task InitializeIfNotExists()
		{
			var appliedMigrations = await _dbContext.Database.GetAppliedMigrationsAsync();
			var databaseExists = appliedMigrations.Any();

			if (databaseExists)
			{
				return;
			}

			_dbContext.Database.Migrate();
			await SeedDbAsync();
		}

		private async Task SeedDbAsync()
		{
			await SeedUsersAsync();

			if (_webHostEnvironment.IsProduction())
			{
				return;
			}

			await SeedBoardsAsync();
			await SeedThreadsAsync();
			await SeedThreadImagesAsync();
			await SeedPostsAsync();
			await SeedPostImagesAsync();

			System.Diagnostics.Debug.WriteLine("Database initialized and seeded.");
		}

		private async Task SeedUsersAsync()
		{
			await _userManager.CreateAsync(
				user: new User(_configuration[AppSettings.DefaultAdminUserName]!),
				password: _configuration[AppSettings.DefaultAdminPassword]);

			if (_webHostEnvironment.IsProduction())
			{
				return;
			}

			// Seed other users for development purposes
		}

		private async Task SeedBoardsAsync()
		{
			var boards = LoadFromJson<List<Board>>(Constants.BoardsSeedingFilePath);
			await _dbContext.AddRangeAsync(boards);
			await _dbContext.SaveChangesAsync();
		}

		private async Task SeedThreadsAsync()
		{
			var boards = await _dbContext.Boards.ToListAsync();
			var threads = LoadFromJson<List<ThreadModel>>(Constants.ThreadsSeedingFilePath);
			boards.First().Threads = threads;
			await _dbContext.SaveChangesAsync();
		}

		private async Task SeedThreadImagesAsync()
		{
			var threads = await _dbContext.Threads.ToListAsync();

			threads[0].ThreadImages = await _fileSeedingService.GetFilesForSeeding<ThreadImage>(1);
			threads[1].ThreadImages = await _fileSeedingService.GetFilesForSeeding<ThreadImage>(2);
			threads[2].ThreadImages = await _fileSeedingService.GetFilesForSeeding<ThreadImage>(3);
			threads[3].ThreadImages = await _fileSeedingService.GetFilesForSeeding<ThreadImage>(5);

			_dbContext.Threads.UpdateRange(threads);
			await _dbContext.SaveChangesAsync();
		}

		private async Task SeedPostsAsync()
		{
			var threads = await _dbContext.Threads.ToListAsync();
			var posts = LoadFromJson<List<Post>>(Constants.PostsSeedingFilePath);

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

			posts[0].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(1);
			posts[1].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(2);
			posts[2].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(3);
			posts[3].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(5);
			posts[4].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(1);
			posts[5].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(2);
			posts[6].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(3);
			posts[7].PostImages = await _fileSeedingService.GetFilesForSeeding<PostImage>(5);

			_dbContext.Posts.UpdateRange(posts);
			await _dbContext.SaveChangesAsync();
		}

		private T LoadFromJson<T>(string path)
		{
			var data = File.ReadAllText(path);
			return JsonSerializer.Deserialize<T>(data, JsonSerializerOptions)!;
		}
	}
}