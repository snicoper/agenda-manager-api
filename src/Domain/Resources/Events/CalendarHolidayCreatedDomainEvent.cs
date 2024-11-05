using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Resources.Events;

public record CalendarHolidayCreatedDomainEvent(CalendarHolidayId CalendarHolidayId) : IDomainEvent;
