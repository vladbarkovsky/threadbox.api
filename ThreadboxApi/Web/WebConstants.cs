namespace ThreadboxApi.Web
{
    public class WebConstants
    {
        /// <summary>
        /// HTTP request URL to get file by <see cref="ORM.Entities.FileInfo"/> <br/>
        /// </summary>
        public const string FileUrl = "{0}/Files/GetFile?fileInfoId={1}";
    }
}