using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common.Helpers.Validation;
using ThreadboxApi.Application.Services;

namespace ThreadboxApi.Application.Identity.Commands
{
    public class SignIn : IRequestHandler<SignIn.Command, string>
    {
        public class Command : IRequest<string>
        {
            public string UserName { get; set; }
            public string Password { get; set; }

            public class Validator : AbstractValidator<Command>
            {
                public Validator()
                {
                    RuleFor(x => x.UserName).ValidateUserName();
                    RuleFor(x => x.Password).ValidatePassword();
                }
            }
        }

        private readonly IdentityService _identityService;

        public SignIn(IdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var accessToken = await _identityService.SignIn(request.UserName, request.Password);
            return accessToken;
        }
    }
}