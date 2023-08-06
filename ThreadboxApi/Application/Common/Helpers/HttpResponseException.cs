namespace ThreadboxApi.Application.Common.Helpers
{
    public class HttpResponseException : Exception
    {
        public static HttpResponseException BadRequest => new("Bad Request", StatusCodes.Status400BadRequest);
        public static HttpResponseException NotFound => new("Not Found", StatusCodes.Status404NotFound);
        public static HttpResponseException Unauthorized => new("Unauthorized", StatusCodes.Status401Unauthorized);

        public int StatusCode { get; set; }

        public HttpResponseException(string message, int statusCode = StatusCodes.Status400BadRequest)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public static void ThrowNotFoundIfNull<T>(T data)
        {
            if (data == null)
            {
                throw new HttpResponseException($"{typeof(T)} not found.", StatusCodes.Status404NotFound);
            }
        }
    }
}