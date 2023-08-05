using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Identity.Commands
{
    public class SignUp : IRequestHandler<SignUp.Command, Unit>
    {
        public class Command : IRequest<Unit>
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string RepeatPassword { get; set; }
            public Guid RegistrationKeyId { get; set; }
        }

        private readonly IdentityService _identityService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTimeService _dateTimeService;
        private readonly IConfiguration _configuration;

        public SignUp(
            IdentityService identityService,
            ApplicationDbContext dbContext,
            IDateTimeService dateTimeService,
            IConfiguration configuration)
        {
            _identityService = identityService;
            _dbContext = dbContext;
            _dateTimeService = dateTimeService;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            await UseRegistrationKey(request.RegistrationKeyId, cancellationToken);
            var user = await _identityService.SignUp(request.UserName, request.Password);

            var person = new Person
            {
                UserName = user.UserName,
                UserId = user.Id
            };

            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task UseRegistrationKey(Guid registrationKeyId, CancellationToken cancellationToken)
        {
            var registrationKey = await _dbContext.RegistrationKeys
                .AsNoTracking()
                .Where(x => x.Id == registrationKeyId)
                .FirstOrDefaultAsync(cancellationToken);

            if (registrationKey == null)
            {
                throw new HttpResponseException("Registration key not found.", StatusCodes.Status400BadRequest);
            }

            var utcNow = _dateTimeService.UtcNow;
            var registrationKeyExpirationTimeSeconds = Convert.ToInt32(_configuration[AppSettings.RegistrationKeyExpirationTimeSeconds]);

            _dbContext.RegistrationKeys.Remove(registrationKey);

            if (utcNow > registrationKey.CreatedAt.AddSeconds(registrationKeyExpirationTimeSeconds))
            {
                throw new HttpResponseException("Registration key expired.", StatusCodes.Status400BadRequest);
            }
        }
    }
}