using MediatR;
using ThreadboxApi.Application.Common.Interfaces;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Identity.Commands
{
    public class CreateRegistrationKey : IRequestHandler<CreateRegistrationKey.Command, Guid>
    {
        public class Command : IRequest<Guid>
        { }

        private readonly ApplicationDbContext _dbContext;
        private readonly IDateTimeService _dateTimeOffsetService;

        public CreateRegistrationKey(ApplicationDbContext dbContext, IDateTimeService dateTimeOffsetService)
        {
            _dbContext = dbContext;
            _dateTimeOffsetService = dateTimeOffsetService;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var registrationKey = new RegistrationKey
            {
                Id = Guid.NewGuid(),
                CreatedAt = _dateTimeOffsetService.UtcNow
            };

            _dbContext.RegistrationKeys.Add(registrationKey);
            await _dbContext.SaveChangesAsync();
            return registrationKey.Id;
        }
    }
}