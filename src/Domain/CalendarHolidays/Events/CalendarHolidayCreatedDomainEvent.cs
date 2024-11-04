using AgendaManager.Domain.CalendarHolidays.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.CalendarHolidays.Events;

public record CalendarHolidayCreatedDomainEvent(CalendarHolidayId CalendarHolidayId) : IDomainEvent;
