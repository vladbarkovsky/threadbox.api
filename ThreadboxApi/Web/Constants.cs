namespace ThreadboxApi.Web
{
    public class Constants
    {
        /// <summary>
        /// HTTP request URL to get file by <see cref="Domain.Entities.FileInfo"/> <br/>
        /// </summary>
        public const string PostImageRequestUrl = "/Files/GetFile?fileInfoId={0}";
    }
}