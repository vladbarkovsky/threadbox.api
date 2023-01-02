using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Dtos;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class DbSeedingService : ITransientService
	{
		private readonly UserManager<User> _userManager;
		private readonly ThreadboxDbContext _dbContext;
		private readonly ImagesService _imagesService;
		private readonly IMapper _mapper;

		public DbSeedingService(IServiceProvider services)
		{
			_userManager = services.GetRequiredService<UserManager<User>>();
			_dbContext = services.GetRequiredService<ThreadboxDbContext>();
			_imagesService = services.GetRequiredService<ImagesService>();
			_mapper = services.GetRequiredService<IMapper>();
		}

		public async Task SeedDbAsync()
		{
			await SeedUsersAsync();
			await SeedBoardsAsync();
			await SeedThreadsAsync();
			await SeedThreadImagesAsync();
			await SeedPostsAsync();
			await SeedPostImagesAsync();
		}

		private async Task SeedUsersAsync()
		{
			if (await _userManager.Users.AnyAsync())
			{
				return;
			}

			await _userManager.CreateAsync(new User("admin"), "P@ssw0rd");
		}

		private async Task SeedBoardsAsync()
		{
			if (await _dbContext.Boards.AnyAsync())
			{
				return;
			}

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
			if (await _dbContext.Threads.AnyAsync())
			{
				return;
			}

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
			if (await _dbContext.ThreadImages.AnyAsync())
			{
				return;
			}

			var threads = await _dbContext.Threads.ToListAsync();
			var images = await _imagesService.GetImagesForSeeding(0, 11);
			var threadImages = _mapper.Map<List<ThreadImage>>(images);

			threads[0].ThreadImages = threadImages.GetRange(0, 1);
			threads[1].ThreadImages = threadImages.GetRange(1, 2);
			threads[2].ThreadImages = threadImages.GetRange(3, 3);
			threads[3].ThreadImages = threadImages.GetRange(6, 5);

			_dbContext.Threads.UpdateRange(threads);
			await _dbContext.ThreadImages.AddRangeAsync(threadImages);
			await _dbContext.SaveChangesAsync();
		}

		private async Task SeedPostsAsync()
		{
			if (await _dbContext.Posts.AnyAsync())
			{
				return;
			}

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
			if (await _dbContext.PostImages.AnyAsync())
			{
				return;
			}

			var posts = await _dbContext.Posts.ToListAsync();
			var images = await _imagesService.GetImagesForSeeding(11, 22);
			var postImages = _mapper.Map<List<PostImage>>(images);

			posts[0].PostImages = postImages.GetRange(0, 1);
			posts[1].PostImages = postImages.GetRange(1, 2);
			posts[2].PostImages = postImages.GetRange(3, 3);
			posts[3].PostImages = postImages.GetRange(6, 5);
			posts[4].PostImages = postImages.GetRange(11, 1);
			posts[5].PostImages = postImages.GetRange(12, 2);
			posts[6].PostImages = postImages.GetRange(14, 3);
			posts[7].PostImages = postImages.GetRange(17, 5);

			_dbContext.Posts.UpdateRange(posts);
			await _dbContext.PostImages.AddRangeAsync(postImages);
			await _dbContext.SaveChangesAsync();
		}
	}
}