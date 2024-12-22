using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarById;

[Authorize(Permissions = SystemPermissions.Calendars.Read)]
public record GetCalendarByIdQuery(Guid CalendarId) : IQuery<GetCalendarByIdQueryResponse>;
