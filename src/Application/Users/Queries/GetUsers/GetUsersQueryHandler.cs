using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.Queries.GetUsers;

internal class GetUsersQueryHandler(IUserRepository usersRepository)
    : IQueryHandler<GetUsersQuery, List<GetUsersQueryResponse>>
{
    public Task<Result<List<GetUsersQueryResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = usersRepository
            .GetQueryable()
            .Select(u => new GetUsersQueryResponse(u.Id.Value, u.Email.Value, u.UserName, u.FirstName, u.LastName))
            .ToList();

        var result = Result.Success(users);

        return Task.FromResult(result);
    }
}
