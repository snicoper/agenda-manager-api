using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Queries.GetRoleWithPermissionAvailabilityById;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetRoleWithPermissionAvailabilityByIdQuery(Guid RoleId)
    : IQuery<GetRoleWithPermissionAvailabilityByIdQueryResponse>;
