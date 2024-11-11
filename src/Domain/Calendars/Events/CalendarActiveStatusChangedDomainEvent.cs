using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Calendars.Events;

public record CalendarActiveStatusChangedDomainEvent(CalendarId Id, bool NewState) : IDomainEvent;
