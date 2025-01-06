using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Resources.Queries.GetResourceById;

[Authorize(Permissions = SystemPermissions.Resources.Read)]
public record GetResourceByIdQuery(Guid ResourceId) : IQuery<GetResourceByIdQueryResponse>;
