﻿using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects;

public sealed record Period
{
    private Period(DateTimeOffset start, DateTimeOffset end)
    {
        ArgumentNullException.ThrowIfNull(start);
        ArgumentNullException.ThrowIfNull(end);

        if (Start > End)
        {
            throw new DomainException($"{nameof(start)} cannot be greater than {nameof(end)}");
        }

        Start = start;
        End = end;
    }

    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    public TimeSpan Duration => End - Start;

    public static Period From(DateTimeOffset start, DateTimeOffset end)
    {
        return new Period(start, end);
    }

    public bool Overlaps(Period other)
    {
        return Start <= other.End && End >= other.Start;
    }

    public bool IsDateInRange(DateTimeOffset date)
    {
        return date >= Start && date <= End;
    }

    public bool IsPeriodInRange(Period period)
    {
        return period.Start >= Start && period.End <= End;
    }

    public int DurationInDays()
    {
        return Duration.Days + 1;
    }
}
