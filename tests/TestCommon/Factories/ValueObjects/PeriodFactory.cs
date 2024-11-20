using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.TestCommon.Factories.ValueObjects;

public static class PeriodFactory
{
    public static Period Create(DateTimeOffset? start = null, DateTimeOffset? end = null)
    {
        return Period.From(start ?? DateTimeOffset.UtcNow, end ?? DateTimeOffset.UtcNow.AddDays(1));
    }
}
