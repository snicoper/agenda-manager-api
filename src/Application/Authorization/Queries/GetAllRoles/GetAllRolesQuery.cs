using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Queries.GetAllRoles;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetAllRolesQuery : IQuery<IEnumerable<GetAllRolesQueryResponse>>;
