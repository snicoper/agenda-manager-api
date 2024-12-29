using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypeById;

[Authorize(Permissions = SystemPermissions.ResourceTypes.Read)]
public record GetResourceTypeByIdQuery(Guid ResourceTypeId) : IQuery<GetResourceTypeByIdQueryResponse>;
