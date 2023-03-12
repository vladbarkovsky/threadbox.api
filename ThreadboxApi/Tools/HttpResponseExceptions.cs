namespace ThreadboxApi.Tools
{
	public class HttpResponseException : Exception
	{
		public HttpErrorResponseCode ResponseCode { get; set; }

		public HttpResponseException(string message, HttpErrorResponseCode responseCode = HttpErrorResponseCode.BadRequest)
			: base(message)
		{
			ResponseCode = responseCode;
		}
	}

	public class HttpResponseExceptions
	{
		public static HttpResponseException BadRequest => new("Bad Request", HttpErrorResponseCode.BadRequest);
		public static HttpResponseException NotFound => new("Not Found", HttpErrorResponseCode.NotFound);

		public static void ThrowNotFoundIfNull(object data)
		{
			if (data == null)
			{
				throw new HttpResponseException($"{data.GetType().Name} not found.", HttpErrorResponseCode.NotFound);
			}
		}
	}

	public enum HttpErrorResponseCode
	{
		BadRequest = StatusCodes.Status400BadRequest,
		NotFound = StatusCodes.Status404NotFound
	}
}