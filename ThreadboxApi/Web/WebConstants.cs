using ThreadboxApi.Application.Files.Queries;
using ThreadboxApi.Web.Controllers;

namespace ThreadboxApi.Web
{
    public class WebConstants
    {
        /// <summary>
        /// HTTP request URL to get file by <see cref="ORM.Entities.FileInfo"/> <br/>
        /// </summary>
        public const string FileUrlTemplate = $"{{0}}/api/Files/{nameof(FileController.Get)}?{nameof(GetFile.Query.FileInfoId)}={{1}}";
    }
}