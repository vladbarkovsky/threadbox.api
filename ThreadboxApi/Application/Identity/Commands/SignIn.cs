﻿using FluentValidation;
using MediatR;
using ThreadboxApi.Application.Common.Helpers.Validation;

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

        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}