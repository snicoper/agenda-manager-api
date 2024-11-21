using AgendaManager.Application.Common.Interfaces.Clock;

namespace AgendaManager.WebApi.UnitTests;

public class TestDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTime.UtcNow;
}
