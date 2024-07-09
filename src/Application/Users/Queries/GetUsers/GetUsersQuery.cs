using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Queries.GetUsers;

public record GetUsersQuery(string Email) : IQuery<List<GetUsersQueryResponse>>;
