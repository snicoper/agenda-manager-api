using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Queries.GetRolesPaginated;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetRolesPaginatedQuery(RequestData RequestData) : IQuery<ResponseData<GetRolesPaginatedQueryResponse>>;
