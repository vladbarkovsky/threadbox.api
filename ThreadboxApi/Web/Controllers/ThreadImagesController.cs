using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace ThreadboxApi.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThreadImagesController : ControllerBase
    {
        private readonly FileService _filesService;

        public ThreadImagesController(FileService filesService)
        {
            _filesService = filesService;
        }

        [HttpGet("[action]")]
        public async Task<FileContentResult> GetThreadImage(Guid imageId)
        {
            var image = await _filesService.GetFileAsync<ThreadImage>(imageId);
            return File(image.Data, image.ContentType, image.Name);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetThreadImages(Guid threadId)
        {
            var archive = await _filesService.GetThreadImagesAsync(threadId);
            return File(archive, Application.Zip, $"Thread_{threadId}_images.zip");
        }
    }
}