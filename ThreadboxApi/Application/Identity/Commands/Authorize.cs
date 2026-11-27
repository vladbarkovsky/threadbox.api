using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Identity;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web.Constants;

namespace ThreadboxApi.Application.Identity.Commands
{
    public class Authorize : IRequestHandler<Authorize.Command, IActionResult>
    {
        public class Command : IRequest<IActionResult> { }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;
        private readonly ILogger<Authorize> _logger;

        public Authorize(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            IOptionsSnapshot<AppSettings> appSettings,
            ILogger<Authorize> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _appSettings = appSettings;
            _logger = logger;
        }

        public async Task<IActionResult> Handle(Command request, CancellationToken cancellationToken)
        {
            OpenIddictRequest openIddictRequest = _httpContextAccessor.HttpContext.GetOpenIddictServerRequest();

            if (openIddictRequest == null)
            {
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
            }

            AuthenticateResult authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            if (!authenticateResult.Succeeded)
            {
                HttpRequest httpRequest = _httpContextAccessor.HttpContext.Request;

                IEnumerable<KeyValuePair<string, StringValues>> parameters = httpRequest.HasFormContentType
                    ? httpRequest.Form
                    : httpRequest.Query;

                return new ChallengeResult(
                    authenticationScheme: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = httpRequest.PathBase + httpRequest.Path + QueryString.Create(parameters)
                    });
            }

            ApplicationUser user = await _userManager.GetUserAsync(authenticateResult.Principal);

            if (user == null)
            {
                _logger.LogError("Authorization request failed because the authenticated user cannot be retrieved.");
                return new RedirectResult(_appSettings.Value.FrontendBaseUrl + UrlConstants.AuthorizationErrorPath);
            }

            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: OpenIddictConstants.Claims.Name,
                roleType: OpenIddictConstants.Claims.Role);

            identity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id);
            identity.SetClaim(OpenIddictConstants.Claims.Name, user.UserName);
            identity.SetScopes(openIddictRequest.GetScopes());
            identity.SetResources(ApplicationOidcConstants.ApiAudience);
            identity.SetAudiences(ApplicationOidcConstants.ApiAudience);
            identity.SetDestinations(GetDestinations);

            return new Microsoft.AspNetCore.Mvc.SignInResult(
                authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                principal: new ClaimsPrincipal(identity));
        }

        private static IEnumerable<string> GetDestinations(Claim claim)
        {
            if (claim.Type == OpenIddictConstants.Claims.Subject)
            {
                yield return OpenIddictConstants.Destinations.AccessToken;
                yield return OpenIddictConstants.Destinations.IdentityToken;
                yield break;
            }

            if (claim.Type == OpenIddictConstants.Claims.Name)
            {
                yield return OpenIddictConstants.Destinations.AccessToken;

                if (claim.Subject.HasScope(OpenIddictConstants.Scopes.Profile))
                {
                    yield return OpenIddictConstants.Destinations.IdentityToken;
                }

                yield break;
            }

            yield return OpenIddictConstants.Destinations.AccessToken;
        }
    }
}
