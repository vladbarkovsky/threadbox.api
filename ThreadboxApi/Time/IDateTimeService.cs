using ThreadboxApi.ServiceLifetimes;

namespace ThreadboxApi.Time
{
    public interface IDateTimeService : IScoped
    {
        DateTimeOffset UtcNow { get; }
    }
}