using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Files.Queries;

namespace ThreadboxApi.Web.Controllers
{
    public class FilesController : MediatRController
    {
        [HttpGet("[action]")]
        public async Task<FileContentResult> GetFile([FromQuery] GetFile.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<FileContentResult> GetThreadImagesZip([FromQuery] GetTreadImagesZip.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<FileContentResult> GetPostImagesZip([FromQuery] GetPostImagesZip.Query query)
        {
            return await Mediator.Send(query);
        }
    }
}