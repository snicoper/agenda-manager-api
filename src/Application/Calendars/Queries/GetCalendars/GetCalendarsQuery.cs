using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Queries.GetCalendars;

[Authorize(Permissions = SystemPermissions.Calendars.Read)]
public record GetCalendarsQuery : IQuery<List<GetCalendarsQueryResponse>>;
