using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Users.Accounts.Queries.GetUsersPaginated;

public record GetUsersPaginatedQuery(RequestData RequestData) : IQuery<List<GetUsersPaginatedQueryResponse>>;
