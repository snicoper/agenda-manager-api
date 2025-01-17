using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceId;

[Authorize(Permissions = SystemPermissions.ResourceSchedules.Read)]
public record GetSchedulesByResourceIdQuery(Guid ResourceId)
    : IQuery<ICollection<GetSchedulesByResourceIdQueryResponse>>;
