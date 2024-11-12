using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Calendars.Events;

public record CalendarSettingsCreatedDomainEvent(CalendarSettings CalendarSettings) : IDomainEvent;
