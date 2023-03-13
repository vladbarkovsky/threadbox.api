namespace ThreadboxApi.Configuration
{
	public class JwtAccessTokenRefreshMiddleware
	{
		private readonly RequestDelegate _requestDelegate;

		public JwtAccessTokenRefreshMiddleware(RequestDelegate requestDelegate)
		{
			_requestDelegate = requestDelegate;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			ExecuteOperations(httpContext);
			await _requestDelegate(httpContext);
		}

		private void ExecuteOperations(HttpContext httpContext)
		{
			var authorizationHeader = httpContext.Request.Headers.Authorization;

			if (!authorizationHeader.Any())
			{
				return;
			}

			var accessToken = authorizationHeader.FirstOrDefault();
			System.Diagnostics.Debug.WriteLine(accessToken);
		}
	}
}