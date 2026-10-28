using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Permissions;
using ThreadboxApi.Application.Threads.Commands;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.Application.Threads.Queries;
using ThreadboxApi.Web.PermissionHandling;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/threads")]
    public sealed class ThreadController : ApplicationController
    {
        public ThreadController(ISender sender) : base(sender) { }

        [HttpGet("{id:guid}")]
        public async Task<ThreadDto> Get([FromRoute] GetThread.Query query)
        {
            return await _sender.Send(query);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ThreadDto), StatusCodes.Status201Created)]
        public async Task<CreatedAtActionResult> Create([FromForm] CreateThread.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpDelete("{id:guid}")]
        [Permission(ThreadsPermissions.Delete)]
        public async Task<NoContentResult> Delete([FromRoute] DeleteThread.Command command)
        {
            return await _sender.Send(command);
        }

        [HttpGet("{threadId:guid}/images")]
        [ExposeContentDisposition]
        public async Task<FileContentResult> GetImagesZip([FromRoute] GetThreadImagesZip.Query query)
        {
            return await _sender.Send(query);
        }
    }
}