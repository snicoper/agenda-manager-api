using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceIdPaginated;

[Authorize(Permissions = SystemPermissions.ResourceSchedules.Read)]
public record GetSchedulesByResourceIdPaginatedQuery(Guid ResourceId, RequestData RequestData)
    : IQuery<ResponseData<GetSchedulesByResourceIdPaginatedQueryResponse>>;
