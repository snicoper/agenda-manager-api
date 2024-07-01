namespace AgendaManager.Application.Common.Interfaces.Clock;

public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
