namespace ThreadboxApi.Application.Services.Interfaces
{
    public interface IDateTimeService : IScopedService
    {
        DateTimeOffset UtcNow { get; }
    }
}