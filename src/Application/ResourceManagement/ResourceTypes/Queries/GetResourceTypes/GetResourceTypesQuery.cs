using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypes;

[Authorize(Permissions = SystemPermissions.ResourceTypes.Read)]
public record GetResourceTypesQuery : IQuery<List<GetResourceTypesQueryResponse>>;
