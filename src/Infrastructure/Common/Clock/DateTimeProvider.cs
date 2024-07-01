using AgendaManager.Application.Common.Interfaces.Clock;

namespace AgendaManager.Infrastructure.Common.Clock;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
