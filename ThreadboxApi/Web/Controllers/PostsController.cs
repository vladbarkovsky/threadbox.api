using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Posts.Commands;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.Posts.Queries;

namespace ThreadboxApi.Web.Controllers
{
    public class PostsController : MediatRController
    {
        [HttpGet("[action]")]
        public async Task<ActionResult<List<PostDto>>> GetPosts([FromQuery] GetPosts.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreatePost([FromForm] CreatePost.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}