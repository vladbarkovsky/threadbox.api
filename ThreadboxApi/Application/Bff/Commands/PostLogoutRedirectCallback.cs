using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class PostLogoutRedirectCallback : IRequestHandler<PostLogoutRedirectCallback.Command, RedirectResult>
    {
        public class Command : IRequest<RedirectResult> { }

        private readonly BffService _bffService;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public PostLogoutRedirectCallback(BffService bffService, IOptionsSnapshot<AppSettings> appSettings)
        {
            _bffService = bffService;
            _appSettings = appSettings;
        }

        public async Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            await _bffService.ClearTokensAsync(cancellationToken);
            return new RedirectResult(_appSettings.Value.FrontendBaseUrl);
        }
    }
}
