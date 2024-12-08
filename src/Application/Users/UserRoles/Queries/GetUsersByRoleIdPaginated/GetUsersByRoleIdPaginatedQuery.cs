using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersByRoleIdPaginated;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetUsersByRoleIdPaginatedQuery(Guid RoleId, RequestData RequestData)
    : IQuery<ResponseData<GetUsersByRoleIdPaginatedQueryResponse>>;
