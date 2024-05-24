namespace ThreadboxApi.Web.Exceptions
{
    public class HttpResponseException : HttpStatusException
    {
        public HttpResponseException(string message, int statusCode = StatusCodes.Status400BadRequest) : base(message, statusCode)
        { }
    }
}