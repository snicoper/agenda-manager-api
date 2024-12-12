using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountDetails;

[Authorize(Permissions = SystemPermissions.Users.Read)]
public record GetAccountDetailsQuery(Guid UserId) : IQuery<GetAccountDetailsQueryResponse>;
