using AgendaManager.Domain.CalendarEvents.Events;
using AgendaManager.Domain.CalendarEvents.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.CalendarEvents;

public class CalendarEvent : AggregateRoot
{
    private CalendarEvent()
    {
    }

    private CalendarEvent(CalendarEventId calendarEventId)
    {
        Id = calendarEventId;
    }

    public CalendarEventId Id { get; } = null!;

    public static CalendarEvent Create(CalendarEventId calendarEventId)
    {
        CalendarEvent calendarEvent = new(calendarEventId);

        calendarEvent.AddDomainEvent(new CalendarEventCreatedDomainEvent(calendarEvent.Id));

        return calendarEvent;
    }
}
