using AgendaManager.Domain.CalendarEvent.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.CalendarEvent.Events;

public record CalendarEventCreatedDomainEvent(CalendarEventId CalendarEventId) : IDomainEvent;
