using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Services;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Bff.Commands
{
    public class Logout : IRequestHandler<Logout.Command, RedirectResult>
    {
        public class Command : IRequest<RedirectResult> { }

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BffService _bffService;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public Logout(
            SignInManager<ApplicationUser> signInManager,
            BffService bffService,
            IOptionsSnapshot<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _bffService = bffService;
            _appSettings = appSettings;
        }

        public async Task<RedirectResult> Handle(Command request, CancellationToken cancellationToken)
        {
            await _bffService.ClearTokensAsync(cancellationToken);
            await _signInManager.SignOutAsync();

            return new RedirectResult(_appSettings.Value.FrontendBaseUrl + "/logged-out");
        }
    }
}
