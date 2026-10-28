using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Files.Queries;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Posts.Commands;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.Posts.Queries;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/posts")]
    public sealed class PostController : ApplicationController
    {
        public PostController(ISender sender) : base(sender) { }

        [HttpGet]
        public async Task<PostDto[]> GetList([FromQuery] GetPostList.Query query)
        {
            return await _sender.Send(query);
        }

        [HttpGet("{id:guid}")]
        public async Task<PostDto> Get([FromRoute] GetPost.Query query)
        {
            return await _sender.Send(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PostDto), StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create([FromForm] CreatePost.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpDelete("{id:guid}")]
        [Permission(PostsPermissions.Delete)]
        public async Task<NoContentResult> Delete([FromRoute] DeletePost.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpGet("{postId:guid}/images")]
        [ExposeContentDisposition]
        public async Task<FileContentResult> GetImagesZip([FromRoute] GetPostImagesZip.Query query)
        {
            return await _sender.Send(query);
        }
    }
}
