using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Domain.Calendars.Entities;

public class CalendarHoliday : AggregateRoot
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

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public static CalendarHoliday Create(
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
}
