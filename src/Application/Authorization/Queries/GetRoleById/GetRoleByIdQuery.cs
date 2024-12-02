using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Queries.GetRoleById;

[Authorize(Permissions = SystemPermissions.Roles.Read)]
public record GetRoleByIdQuery(Guid Id) : IQuery<GetRoleByIdQueryResponse>;
