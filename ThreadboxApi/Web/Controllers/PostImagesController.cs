using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Domain.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace ThreadboxApi.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostImagesController : ControllerBase
    {
        private readonly FileService _filesService;

        public PostImagesController(FileService filesService)
        {
            _filesService = filesService;
        }

        [HttpGet("[action]")]
        public async Task<FileContentResult> GetPostImage(Guid imageId)
        {
            var image = await _filesService.GetFileAsync<PostImage>(imageId);
            return File(image.Data, image.ContentType, image.Name);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetPostImages(Guid postId)
        {
            var archive = await _filesService.GetPostImagesAsync(postId);
            return File(archive, Application.Zip, $"Post_{postId}_images.zip");
        }
    }
}