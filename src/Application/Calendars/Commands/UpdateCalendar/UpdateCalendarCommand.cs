using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendar;

[Authorize(Permissions = SystemPermissions.Calendars.Update)]
public record UpdateCalendarCommand(Guid CalendarId, string Name, string Description) : ICommand;
