using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects;

public sealed record TimeSlot
{
    private TimeSlot(TimeOnly start, TimeOnly end)
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(end);

        if (start > end)
        {
            throw new DomainException($"{nameof(start)} cannot be greater than {nameof(end)}");
        }

        Start = start;
        End = end;
    }

    public TimeOnly Start { get; }

    public TimeOnly End { get; }

    public static TimeSlot From(TimeOnly start, TimeOnly end)
    {
        return new TimeSlot(start, end);
    }

    public bool Overlaps(TimeOnly other)
    {
        return Start <= other && End >= other;
    }

    public bool IsDateInRange(TimeOnly time)
    {
        return time >= Start && time <= End;
    }

    public int DurationInDays()
    {
        return (int)(End - Start).TotalDays + 1;
    }
}
