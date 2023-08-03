using MediatR;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Identity.Commands
{
    public class RefreshAccessToken : IRequestHandler<RefreshAccessToken.Command, string>
    {
        public class Command : IRequest<string>
        { }

        private readonly JwtService _jwtService;
        private readonly ApplicationContext _appContext;

        public RefreshAccessToken(JwtService jwtService, ApplicationContext appContext)
        {
            _jwtService = jwtService;
            _appContext = appContext;
        }

        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _appContext.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                throw HttpResponseException.Unauthorized;
            }

            var accessToken = _jwtService.CreateAccessToken(userId);
            return await Task.FromResult(accessToken);
        }
    }
}