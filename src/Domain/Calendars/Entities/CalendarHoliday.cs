using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarHoliday : AggregateRoot
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
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

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

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    internal static CalendarHoliday Create(
        CalendarHolidayId calendarHolidayId,
        CalendarId calendarId,
        Period period,
        WeekDays weekDays,
        string name,
        string description)
    {
        CalendarHoliday calendarHoliday = new(
            calendarHolidayId,
            calendarId,
            period,
            weekDays,
            name,
            description);

        calendarHoliday.AddDomainEvent(new CalendarHolidayCreatedDomainEvent(calendarHoliday.Id));

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
