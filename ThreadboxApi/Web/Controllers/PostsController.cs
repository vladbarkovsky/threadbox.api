using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Posts.Commands;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.Posts.Queries;

namespace ThreadboxApi.Web.Controllers
{
    public class PostsController : MediatRController
    {
        [HttpGet("[action]")]
        public async Task<ActionResult<List<PostDto>>> GetPostsByThread([FromQuery] GetPostsByThread.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> CreatePost([FromBody] CreatePost.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}