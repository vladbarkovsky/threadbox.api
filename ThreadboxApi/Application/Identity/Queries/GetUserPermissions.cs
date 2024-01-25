using MediatR;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Identity.Queries
{
    public class GetUserPermissions : IRequestHandler<GetUserPermissions.Query, List<string>>
    {
        public class Query : IRequest<List<string>>
        { }

        private readonly IdentityService _identityService;

        public GetUserPermissions(IdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _identityService.GetPermissionsAsync();
        }
    }
}