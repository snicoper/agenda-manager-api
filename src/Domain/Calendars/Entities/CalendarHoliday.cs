using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarHoliday : AuditableEntity
{
    private CalendarHoliday()
    {
    }

    private CalendarHoliday(
        CalendarHolidayId calendarHolidayId,
        CalendarId calendarId,
        Period period,
        string name)
    {
        Id = calendarHolidayId;
        CalendarId = calendarId;
        Period = period;
        Name = name;
    }

    public CalendarHolidayId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    internal static CalendarHoliday Create(
        CalendarHolidayId calendarHolidayId,
        CalendarId calendarId,
        Period period,
        string name)
    {
        GuardAgainstInvalidName(name);

        CalendarHoliday calendarHoliday = new(
            calendarHolidayId: calendarHolidayId,
            calendarId: calendarId,
            period: period,
            name: name);

        calendarHoliday.AddDomainEvent(new CalendarHolidayCreatedDomainEvent(calendarHolidayId));

        return calendarHoliday;
    }

    internal bool Update(Period period, string name)
    {
        GuardAgainstInvalidName(name);

        if (Name == name && Period == period)
        {
            return false;
        }

        Period = period;
        Name = name;

        return true;
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new CalendarHolidayDomainException("Name is invalid or exceeds length of 50 characters.");
        }
    }
}
