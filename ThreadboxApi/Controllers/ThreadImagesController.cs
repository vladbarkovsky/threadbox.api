using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Models;
using ThreadboxApi.Services;
using static System.Net.Mime.MediaTypeNames;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ThreadImagesController : ControllerBase
	{
		private readonly FilesService _filesService;

		public ThreadImagesController(FilesService filesService)
		{
			_filesService = filesService;
		}

		[HttpGet("[action]")]
		public async Task<FileContentResult> GetThreadImage(Guid imageId)
		{
			var image = await _filesService.GetFileAsync<ThreadImage>(imageId);
			return File(image.Data, image.ContentType, image.Name);
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> GetThreadImages(Guid threadId)
		{
			var archive = await _filesService.GetThreadImagesAsync(threadId);
			return File(archive, Application.Zip, $"Thread_{threadId}_images.zip");
		}
	}
}