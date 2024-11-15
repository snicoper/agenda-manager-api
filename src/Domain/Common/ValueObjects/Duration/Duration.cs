namespace AgendaManager.Domain.Common.ValueObjects.Duration;

public sealed record Duration
{
    private Duration(TimeSpan value)
    {
        GuardAgainstDurationException(value);

        Value = value;
    }

    public TimeSpan Value { get; }

    public static Duration From(TimeSpan duration)
    {
        return new Duration(duration);
    }

    private static void GuardAgainstDurationException(TimeSpan duration)
    {
        ArgumentNullException.ThrowIfNull(duration);

        if (duration <= TimeSpan.Zero)
        {
            throw new DurationException("Duration cannot be zero or negative.");
        }
    }
}
