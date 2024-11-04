using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects;

public class Period : ValueObject
{
    private Period(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        StartDate = startDate;
        EndDate = endDate;

        if (StartDate > EndDate)
        {
            throw new InvalidPeriodException("End date must be greater than or equal to start date");
        }
    }

    public DateTimeOffset StartDate { get; }

    public DateTimeOffset EndDate { get; }

    public static Period From(DateTimeOffset start, DateTimeOffset end)
    {
        return new Period(start, end);
    }

    public bool Overlaps(Period other)
    {
        return StartDate <= other.EndDate && EndDate >= other.StartDate;
    }

    public bool IsDateInRange(DateTimeOffset date)
    {
        return date >= StartDate && date <= EndDate;
    }

    public bool IsPeriodInRange(Period period)
    {
        return period.StartDate >= StartDate && period.EndDate <= EndDate;
    }

    public bool IsValid()
    {
        return EndDate >= StartDate;
    }

    public int DurationInDays()
    {
        return (EndDate - StartDate).Days + 1;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
    }
}
