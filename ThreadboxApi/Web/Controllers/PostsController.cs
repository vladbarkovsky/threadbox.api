using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Dtos;

namespace ThreadboxApi.Web.Controllers
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
        public async Task<ActionResult<ListPostDto>> CreatePost(PostDto postDto)
        {
            return new ListPostDto();
        }
    }
}