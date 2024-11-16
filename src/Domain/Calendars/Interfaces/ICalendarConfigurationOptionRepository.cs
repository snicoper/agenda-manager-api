using AgendaManager.Domain.Calendars.Entities;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarConfigurationOptionRepository
{
    Task<List<CalendarConfigurationOption>> GetAllAsync(CancellationToken cancellationToken = default);
}
