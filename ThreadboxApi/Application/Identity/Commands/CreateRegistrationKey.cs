using MediatR;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Identity.Commands
{
    public class CreateRegistrationKey : IRequestHandler<CreateRegistrationKey.Command, Guid>
    {
        public class Command : IRequest<Guid>
        { }

        private readonly ApplicationDbContext _dbContext;

        public CreateRegistrationKey(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var registrationKey = new RegistrationKey
            {
                Id = Guid.NewGuid()
            };

            _dbContext.RegistrationKeys.Add(registrationKey);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return registrationKey.Id;
        }
    }
}