using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Calendars.Events;

public record CalendarSettingsUpdatedDomainEvent(CalendarId CalendarId, CalendarSettingsId CalendarSettingsId)
    : IDomainEvent;
