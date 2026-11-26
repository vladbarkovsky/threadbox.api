namespace ThreadboxApi.Web.Constants
{
    public class UrlConstants
    {
        public const string SignInPath = "/Identity/Account/Login";
        public const string ContinueLoginPath = "/bff/continue-login";
        public const string PostLoginRedirectCallbackPath = "/bff/post-login-redirect-callback";
        public const string ConnectTokenPath = "/connect/token";
        public const string AuthorizationErrorPath = "/authorization-error";
        public const string LoggedOutPath = "/logged-out";
        public const string ApiPath = "/api";
        public const string ApiSpecificationPath = ApiPath + "/specification.json";
    }
}
