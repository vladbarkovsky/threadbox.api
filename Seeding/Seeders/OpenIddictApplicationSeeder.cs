using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Identity;
using ThreadboxApi.Web.Constants;

namespace Seeding.Seeders
{
    public class OpenIddictApplicationSeeder
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public OpenIddictApplicationSeeder(
            IOpenIddictApplicationManager applicationManager,
            IOptionsSnapshot<AppSettings> appSettings)
        {
            _applicationManager = applicationManager;
            _appSettings = appSettings;
        }

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = ApplicationOidcConstants.ClientId,
                ClientSecret = _appSettings.Value.OidcBffClientSecret,
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,
                DisplayName = "BFF",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Permissions.Prefixes.Scope + ApplicationOidcConstants.ApiScope
                },
                RedirectUris =
                {
                    new Uri(_appSettings.Value.BaseUrl + UrlConstants.PostLoginRedirectCallbackPath, UriKind.Absolute)
                }
            };

            await _applicationManager.CreateAsync(descriptor, cancellationToken);
        }
    }
}
