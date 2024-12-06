using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Users.Accounts.Queries.GetUsersPaginated;

internal class
    GetUsersPaginatedQueryHandler : IQueryHandler<GetUsersPaginatedQuery, List<GetUsersPaginatedQueryResponse>>
{
    public Task<Result<List<GetUsersPaginatedQueryResponse>>> Handle(
        GetUsersPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        // var users = userRepository.GetQueryable();
        throw new NotImplementedException();
    }
}
