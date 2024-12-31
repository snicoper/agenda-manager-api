using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Application.Calendars.Commands.UpdateAvailableDays;

[Authorize(Permissions = SystemPermissions.Calendars.Update)]
public record UpdateAvailableDaysCommand(Guid CalendarId, WeekDays AvailableDays) : ICommand;
