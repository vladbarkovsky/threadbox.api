namespace ThreadboxApi.Web.ErrorHandling
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }

        public HttpResponseException(
            string message,
            int statusCode = StatusCodes.Status400BadRequest,
            Exception innerException = null) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}