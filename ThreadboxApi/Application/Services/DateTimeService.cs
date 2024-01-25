using ThreadboxApi.Application.Services.Interfaces;

namespace ThreadboxApi.Application.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}