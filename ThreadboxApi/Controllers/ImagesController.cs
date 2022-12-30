using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Models;
using ThreadboxApi.Services;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ImagesController : ControllerBase
	{
		private readonly ImagesService _imagesService;

		public ImagesController(IServiceProvider services)
		{
			_imagesService = services.GetRequiredService<ImagesService>();
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> GetThreadImage(Guid imageId)
		{
			var image = await _imagesService.TryGetImageAsync<ThreadImage>(imageId);
			return image != null ? File(image.Data, $"image/{image.Extension}") : NotFound();
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> GetPostImage(Guid imageId)
		{
			var image = await _imagesService.TryGetImageAsync<PostImage>(imageId);
			return image != null ? File(image.Data, $"image/{image.Extension}") : NotFound();
		}
	}
}