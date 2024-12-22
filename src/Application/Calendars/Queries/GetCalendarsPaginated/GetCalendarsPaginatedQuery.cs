using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarsPaginated;

[Authorize(Permissions = SystemPermissions.Calendars.Read)]
public record GetCalendarsPaginatedQuery(RequestData RequestData)
    : IQuery<ResponseData<GetCalendarsPaginatedQueryResponse>>;
