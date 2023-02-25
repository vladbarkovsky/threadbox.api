using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Models;
using ThreadboxApi.Services;
using static System.Net.Mime.MediaTypeNames;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ImagesController : ControllerBase
	{
		private readonly ImagesService _imagesService;
		private readonly FilesService _filesService;

		public ImagesController(IServiceProvider services)
		{
			_imagesService = services.GetRequiredService<ImagesService>();
			_filesService = services.GetRequiredService<FilesService>();
		}

		[HttpGet("[action]")]
		public async Task<FileContentResult> GetThreadImage(Guid imageId)
		{
			var image = await _filesService.GetFileAsync<ThreadImage>(imageId);
			return File(image.Data, image.ContentType, image.Name);
		}

		[HttpGet("[action]")]
		public async Task<FileContentResult> GetPostImage(Guid imageId)
		{
			var image = await _filesService.GetFileAsync<PostImage>(imageId);
			return File(image.Data, image.ContentType, image.Name);
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> GetThreadImages(Guid threadId)
		{
			var archive = await _imagesService.GetThreadImagesAsync(threadId);
			return File(archive, Application.Zip, $"Thread_{threadId}_images.zip");
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> GetPostImages(Guid postId)
		{
			var archive = await _imagesService.GetPostImagesAsync(postId);
			return File(archive, Application.Zip, $"Post_{postId}_images.zip");
		}
	}
}