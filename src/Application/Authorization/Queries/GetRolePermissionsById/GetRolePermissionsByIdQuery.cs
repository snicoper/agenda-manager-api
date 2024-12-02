using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Queries.GetRolePermissionsById;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetRolePermissionsByIdQuery(Guid RoleId) : IQuery<GetRolePermissionsByIdQueryResponse>;
