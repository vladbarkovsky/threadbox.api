using MediatR;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Identity.Queries
{
    public class GetUserSession : IRequestHandler<GetUserSession.Query, UserSession>
    {
        public class Query : IRequest<UserSession>
        { }

        private readonly IdentityService _identityService;

        public GetUserSession(IdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserSession> Handle(Query request, CancellationToken cancellationToken)
        {
            string userName = await _identityService.GetUserNameAsync();

            return new UserSession
            {
                UserName = userName
            };
        }
    }
}
