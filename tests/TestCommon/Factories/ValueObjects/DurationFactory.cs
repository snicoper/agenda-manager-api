using AgendaManager.Domain.Common.ValueObjects.Duration;

namespace AgendaManager.TestCommon.Factories.ValueObjects;

public static class DurationFactory
{
    public static Duration Create(TimeSpan? duration = null)
    {
        return Duration.From(duration ?? TimeSpan.FromHours(1));
    }
}
