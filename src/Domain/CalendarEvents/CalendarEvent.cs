using AgendaManager.Domain.CalendarEvents.Events;
using AgendaManager.Domain.CalendarEvents.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.CalendarEvents;

public class CalendarEvent : AggregateRoot
{
    private CalendarEvent()
    {
    }

    private CalendarEvent(CalendarEventId id)
    {
        Id = id;
    }

    public CalendarEventId Id { get; } = null!;

    public static CalendarEvent Create(CalendarEventId id)
    {
        CalendarEvent calendarEvent = new(id);

        calendarEvent.AddDomainEvent(new CalendarEventCreatedDomainEvent(calendarEvent.Id));

        return calendarEvent;
    }
}
