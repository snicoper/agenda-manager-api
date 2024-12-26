using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarSettings;

[Authorize(Permissions = SystemPermissions.CalendarSettings.Read)]
public record GetCalendarSettingsQuery(Guid CalendarId) : IQuery<GetCalendarSettingsQueryResponse>;
