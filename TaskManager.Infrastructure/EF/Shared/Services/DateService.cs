using TaskManager.Core.Shared.Services;

namespace TaskManager.Infrastructure.EF.Shared.Services;

public sealed class DateService : IDateService
{
    public DateTime CurrentDate()
    {
        return DateTime.UtcNow;
    }
}