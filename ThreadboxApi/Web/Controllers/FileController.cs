using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Files.Queries;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/files")]
    public sealed class FileController : ApplicationController
    {
        public FileController(ISender sender) : base(sender) { }

        [HttpGet]
        [ExposeContentDisposition]
        public async Task<FileContentResult> Get([FromQuery] GetFile.Query query)
        {
            return await _sender.Send(query);
        }
    }
}
