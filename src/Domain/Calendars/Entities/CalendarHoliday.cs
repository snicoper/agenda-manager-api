using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;

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
        WeekDays weekDays,
        string name,
        string description)
    {
        Id = calendarHolidayId;
        CalendarId = calendarId;
        Period = period;
        AvailableDays = weekDays;
        Name = name;
        Description = description;
    }

    public CalendarHolidayId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public WeekDays AvailableDays { get; private set; }

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    internal static CalendarHoliday Create(
        CalendarHolidayId calendarHolidayId,
        CalendarId calendarId,
        Period period,
        WeekDays weekDays,
        string name,
        string description)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        CalendarHoliday calendarHoliday = new(
            calendarHolidayId: calendarHolidayId,
            calendarId: calendarId,
            period: period,
            weekDays: weekDays,
            name: name,
            description: description);

        calendarHoliday.AddDomainEvent(new CalendarHolidayCreatedDomainEvent(calendarHolidayId));

        return calendarHoliday;
    }

    internal void Update(Period period, WeekDays weekDays, string name, string description)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Period = period;
        AvailableDays = weekDays;
        Name = name;
        Description = description;

        AddDomainEvent(new CalendarHolidayUpdatedDomainEvent(Id));
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new CalendarHolidayDomainException("Name is invalid or exceeds length of 50 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        ArgumentNullException.ThrowIfNull(description);

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new CalendarHolidayDomainException("Description is invalid or exceeds length of 500 characters.");
        }
    }
}
