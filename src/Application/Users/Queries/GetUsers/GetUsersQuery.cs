using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Security;
using AgendaManager.Domain.Common.Constants;

namespace AgendaManager.Application.Users.Queries.GetUsers;

[Authorize(Roles = Roles.Admin)]
public record GetUsersQuery(string Email) : IQuery<List<GetUsersQueryResponse>>;
