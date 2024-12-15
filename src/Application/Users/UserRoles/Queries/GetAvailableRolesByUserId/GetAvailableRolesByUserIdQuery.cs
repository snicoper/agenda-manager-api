using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetAvailableRolesByUserId;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetAvailableRolesByUserIdQuery(Guid UserId) : IQuery<List<GetAvailableRolesByUserIdQueryResponse>>;
