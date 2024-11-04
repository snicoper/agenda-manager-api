using AgendaManager.Domain.CalendarHolidays.Events;
using AgendaManager.Domain.CalendarHolidays.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

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

    public static CalendarHoliday Create(CalendarHolidayId calendarHolidayId)
    {
        CalendarHoliday calendarHoliday = new(calendarHolidayId);

        calendarHoliday.AddDomainEvent(new CalendarHolidayCreatedDomainEvent(calendarHoliday.Id));

        return calendarHoliday;
    }
}
