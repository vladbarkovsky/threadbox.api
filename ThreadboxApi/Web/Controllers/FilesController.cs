using Microsoft.AspNetCore.Mvc;

namespace ThreadboxApi.Web.Controllers
{
    public class FilesController : MediatRController
    {
        [HttpGet("[action]")]
        public async Task<FileContentResult> GetFile(Guid fileInfoId)
        {
        }
    }
}