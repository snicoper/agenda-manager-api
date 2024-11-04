using AgendaManager.Domain.CalendarEvents.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.CalendarEvents.Events;

public record CalendarEventCreatedDomainEvent(CalendarEventId CalendarEventId) : IDomainEvent;
