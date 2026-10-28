using MediatR;
using ThreadboxApi.Application.Identity.Models;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Identity.Queries
{
    public class GetUserPermissions : IRequestHandler<GetUserPermissions.Query, UserSession>
    {
        public class Query : IRequest<UserSession> { }

        private readonly IdentityService _identityService;

        public GetUserPermissions(IdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserSession> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _identityService.GetUserSessionAsync();
        }
    }
}
