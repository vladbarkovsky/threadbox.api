using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Dtos;
using ThreadboxApi.Services;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PostsController : ControllerBase
	{
		private readonly PostsService _postsService;

		public PostsController(IServiceProvider services)
		{
			_postsService = services.GetRequiredService<PostsService>();
		}

		[HttpPost("[action]")]
		public async Task<ActionResult<ListPostDto>> CreatePost(Guid threadId, PostDto postDto)
		{
			return new ListPostDto();
		}
	}
}