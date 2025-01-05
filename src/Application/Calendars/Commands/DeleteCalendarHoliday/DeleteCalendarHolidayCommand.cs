using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Commands.DeleteCalendarHoliday;

[Authorize(Permissions = SystemPermissions.CalendarHolidays.Delete)]
public record DeleteCalendarHolidayCommand(Guid CalendarId, Guid CalendarHolidayId) : ICommand;
