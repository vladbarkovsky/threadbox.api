using MediatR;
using ThreadboxApi.Application.Identity.Models;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Identity.Queries
{
    public class GetUserSession : IRequestHandler<GetUserSession.Query, UserSessionDto>
    {
        public class Query : IRequest<UserSessionDto> { }

        private readonly IdentityService _identityService;

        public GetUserSession(IdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserSessionDto> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _identityService.GetUserSessionAsync();
        }
    }
}
