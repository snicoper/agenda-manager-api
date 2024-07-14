using AgendaManager.Application.Common.Interfaces.Clock;

namespace AgendaManager.TestCommon.Services;

public class TestDateTimeProvider(DateTimeOffset? fixedDateTimeOffset = null)
    : IDateTimeProvider
{
    public DateTimeOffset UtcNow => fixedDateTimeOffset ?? DateTimeOffset.UtcNow;
}
