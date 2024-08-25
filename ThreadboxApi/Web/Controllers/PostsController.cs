using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Posts.Commands;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.Posts.Queries;
using ThreadboxApi.Web.PermissionHandling;

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

        [HttpDelete("[action]")]
        [Permission(PostsPermissions.Delete)]
        public async Task<ActionResult> DeletePost([FromQuery] DeletePost.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}