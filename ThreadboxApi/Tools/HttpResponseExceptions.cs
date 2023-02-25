namespace ThreadboxApi.Tools
{
	public class HttpResponseException : Exception
	{
		public int HttpErrorResponseCode { get; set; }

		public HttpResponseException(string message, int httpErrorResponseCode = StatusCodes.Status400BadRequest)
			: base(message)
		{
			HttpErrorResponseCode = httpErrorResponseCode;
		}
	}

	public class HttpResponseExceptions
	{
		public static HttpResponseException BadRequest => new("Bad Request", StatusCodes.Status400BadRequest);
		public static HttpResponseException NotFound => new("Not Found", StatusCodes.Status404NotFound);

		public static void ThrowNotFoundIfNull(object? data)
		{
			if (data == null)
			{
				throw NotFound;
			}
		}
	}
}