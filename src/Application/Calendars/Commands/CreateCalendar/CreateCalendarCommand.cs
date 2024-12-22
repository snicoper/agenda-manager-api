using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Commands.CreateCalendar;

[Authorize(Permissions = SystemPermissions.Calendars.Create)]
public record CreateCalendarCommand(string Name, string Description, string IanaTimeZone)
    : ICommand<CreateCalendarCommandResponse>;
