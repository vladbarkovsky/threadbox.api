using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

		public DbInitializationService(IServiceProvider services)
		{
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();
			_configuration = services.GetRequiredService<IConfiguration>();
			_userManager = services.GetRequiredService<UserManager<User>>();
			_fileSeedingService = services.GetRequiredService<FileSeedingService>();
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

			if (!_webHostEnvironment.IsDevelopment())
			{
				return;
			}

			await SeedBoardsAsync();
			await SeedThreadsAsync();
			await SeedThreadImagesAsync();
			await SeedPostsAsync();
			await SeedPostImagesAsync();
		}

		private async Task SeedUsersAsync()
		{
			await _userManager.CreateAsync(
				user: new User(_configuration[AppSettings.DefaultAdminUserName]!),
				password: _configuration[AppSettings.DefaultAdminPassword]);

			if (!_webHostEnvironment.IsDevelopment())
			{
				return;
			}

			// Seed other users for development purposes
		}

		private async Task SeedBoardsAsync()
		{
			var boards = new List<Board>
			{
				new Board
				{
					Title = "ASP.NET",
					Description = "Active Server Pages for Network Enabled Technologies - open-source, server-side web-application framework designed for web development to produce dynamic web pages. It was developed by Microsoft to allow programmers to build dynamic web sites, applications and services."
				},
				new Board
				{
					Title = "Angular",
					Description = " TypeScript-based, free and open-source web application framework led by the Angular Team at Google and by a community of individuals and corporations."
				},
				new Board
				{
					Title = "NSwag",
					Description = "What is NSwag in .NET core?\r\nNSwag is a Swagger/OpenAPI 2.0 and 3.0 toolchain for . NET, . NET Core, Web API, ASP.NET Core, TypeScript (jQuery, AngularJS, Angular 2+, Aurelia, KnockoutJS and more) and other platforms, written in C#. The OpenAPI/Swagger specification uses JSON and JSON Schema to describe a RESTful web API."
				},
			};

			await _dbContext.AddRangeAsync(boards);
			await _dbContext.SaveChangesAsync();
		}

		private async Task SeedThreadsAsync()
		{
			var boards = await _dbContext.Boards.ToListAsync();

			var threads = new List<ThreadModel>
			{
				new ThreadModel
				{
					Title = "Thread 1",
					Text = "Thread 1 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 2",
					Text = "Thread 2 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 3",
					Text = "Thread 3 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 4",
					Text = "Thread 4 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 5",
					Text = "Thread 5 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 6",
					Text = "Thread 6 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 7",
					Text = "Thread 7 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 8",
					Text = "Thread 8 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 9",
					Text = "Thread 9 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
				new ThreadModel
				{
					Title = "Thread 10",
					Text = "Thread 10 Flee in terror at cucumber discovered on floor. Meow for food, then when human fills food dish, take a few bites of food and continue meowing purr as loud as possible, be the most annoying cat that you can, and, knock everything off the table purr purr purr until owner pets why owner not pet me hiss scratch meow yet stand in doorway,",
				},
			};

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

			var posts = new List<Post>
			{
				new Post
				{
					Text = "Post 1 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 2 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 3 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 4 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 5 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 6 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 7 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 8 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 9 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 10 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				},
				new Post
				{
					Text = "Post 11 Attempt to leap between furniture but woefully miscalibrate and bellyflop onto the floor; what's your problem? i meant to do that now i shall wash myself intently shove bum in owner's face like camera lens get away from me stupid dog, mouse so i is playing on your console hooman. Kitty scratches couch bad kitty fall over dead (not really but gets sypathy) cats are cute."
				}
			};

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
	}
}