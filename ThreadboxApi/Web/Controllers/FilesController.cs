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
    }
}