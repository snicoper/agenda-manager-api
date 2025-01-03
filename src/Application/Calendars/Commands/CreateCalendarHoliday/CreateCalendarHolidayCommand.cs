using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Commands.CreateCalendarHoliday;

[Authorize(Permissions = SystemPermissions.CalendarHolidays.Create)]
public record CreateCalendarHolidayCommand(Guid CalendarId, DateTimeOffset Start, DateTimeOffset End, string Name)
    : ICommand<CreateCalendarHolidayCommandResponse>;
