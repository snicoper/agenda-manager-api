using AgendaManager.Domain.CalendarHolidays.Events;
using AgendaManager.Domain.CalendarHolidays.ValueObjects;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.CalendarHolidays;

public class CalendarHoliday : AggregateRoot
{
    private CalendarHoliday()
    {
    }

    private CalendarHoliday(CalendarHolidayId calendarHolidayId)
    {
        Id = calendarHolidayId;
    }

    public CalendarHolidayId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public static CalendarHoliday Create(CalendarHolidayId calendarHolidayId)
    {
        CalendarHoliday calendarHoliday = new(calendarHolidayId);

        calendarHoliday.AddDomainEvent(new CalendarHolidayCreatedDomainEvent(calendarHoliday.Id));

        return calendarHoliday;
    }
}
