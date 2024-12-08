using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleIdPaginated;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetUsersNotInRoleIdPaginatedCommand(Guid RoleId, RequestData RequestData)
    : IQuery<ResponseData<GetUsersNotInRoleIdPaginatedCommandResponse>>;
