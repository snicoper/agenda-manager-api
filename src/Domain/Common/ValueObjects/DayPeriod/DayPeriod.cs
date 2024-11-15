using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Common.ValueObjects.DayPeriod;

public sealed class DayPeriod : ValueObject
{
    private DayPeriod(DateOnly start, DateOnly end)
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(end);

        if (Start > End)
        {
            throw new InvalidDayPeriodException($"{nameof(end)} cannot be greater than {nameof(start)}");
        }

        Start = start;
        End = end;
    }

    public DateOnly Start { get; }

    public DateOnly End { get; }

    public static DayPeriod From(DateOnly start, DateOnly end)
    {
        return new DayPeriod(start, end);
    }

    public bool Overlaps(DayPeriod other)
    {
        return Start <= other.End && End >= other.Start;
    }

    public bool IsDateInRange(DateOnly date)
    {
        return date >= Start && date <= End;
    }

    public bool IsPeriodInRange(DayPeriod period)
    {
        return period.Start >= Start && period.End <= End;
    }

    public int DurationInDays()
    {
        return End.DayNumber - Start.DayNumber + 1;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}
