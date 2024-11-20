namespace AgendaManager.Domain.Common.ValueObjects.Duration;

public sealed record Duration
{
    private Duration(TimeSpan value)
    {
        GuardAgainstInvalidDuration(value);

        Value = value;
    }

    public TimeSpan Value { get; }

    public static Duration From(TimeSpan duration)
    {
        return new Duration(duration);
    }

    public static Duration From(int hours, int minutes)
    {
        return new Duration(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes));
    }

    public static Duration From(int days, int hours, int minutes)
    {
        return new Duration(TimeSpan.FromDays(days) + TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes));
    }

    private static void GuardAgainstInvalidDuration(TimeSpan duration)
    {
        ArgumentNullException.ThrowIfNull(duration);

        if (duration <= TimeSpan.Zero)
        {
            throw new DurationException("Duration cannot be zero or negative.");
        }
    }
}
