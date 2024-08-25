namespace ThreadboxApi.Web.ErrorHandling
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }

        public HttpResponseException(string message, int statusCode = StatusCodes.Status400BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }

        // TODO: Throw 401 exceptions manually.
        public static void ThrowNotFoundIfNull<T>(T data)
        {
            if (data == null)
            {
                throw new HttpResponseException($"{typeof(T)} not found.", StatusCodes.Status404NotFound);
            }
        }
    }
}