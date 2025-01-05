using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendarHoliday;

[Authorize(Permissions = SystemPermissions.CalendarHolidays.Update)]
public record UpdateCalendarHolidayCommand(
    Guid CalendarId,
    Guid CalendarHolidayId,
    string Name,
    DateTimeOffset Start,
    DateTimeOffset End)
    : ICommand;
