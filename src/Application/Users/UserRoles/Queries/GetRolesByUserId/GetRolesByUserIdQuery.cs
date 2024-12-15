using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetRolesByUserId;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetRolesByUserIdQuery(Guid UserId) : IQuery<List<GetRolesByUserIdQueryResponse>>;
