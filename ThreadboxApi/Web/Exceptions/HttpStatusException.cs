namespace ThreadboxApi.Web.Exceptions
{
    public class HttpStatusException : Exception
    {
        public int StatusCode { get; }

        public HttpStatusException(string message, int statusCode = StatusCodes.Status400BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public static void ThrowNotFoundIfNull<T>(T data)
        {
            if (data == null)
            {
                throw new HttpStatusException($"{typeof(T)} not found.", StatusCodes.Status404NotFound);
            }
        }
    }
}