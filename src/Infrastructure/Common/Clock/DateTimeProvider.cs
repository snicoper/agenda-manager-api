using AgendaManager.Application.Common.Abstractions.Clock;

namespace AgendaManager.Infrastructure.Common.Clock;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
