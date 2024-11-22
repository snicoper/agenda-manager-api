using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Calendars.Policies;

public class CalendarNameValidationPolicy(ICalendarRepository calendarRepository) : ICalendarNameValidationPolicy
{
    public async Task<bool> ExistsAsync(
        CalendarId calendarId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await calendarRepository.ExistsByNameAsync(calendarId, name, cancellationToken);

        return exists;
    }
}
