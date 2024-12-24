using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Commands.ToggleIsActive;

[Authorize(Permissions = SystemPermissions.Calendars.Update)]
public record ToggleIsActiveCommand(Guid CalendarId) : ICommand;
