namespace ThreadboxApi.Web.Exceptions
{
    public class HttpResponseException : HttpStatusException
    {
        public string Response { get; }

        public HttpResponseException(
            string message,
            string response,
            int statusCode = StatusCodes.Status400BadRequest) : base(message, statusCode)
        {
            Response = response;
        }
    }
}