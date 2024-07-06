using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Persistence;

namespace AgendaManager.Application.Users.Queries.GetUsers;

internal class GetUsersHandler(IUsersRepository usersRepository)
    : IQueryHandler<GetUsersQuery, List<GetUsersResponse>>
{
    public Task<Result<List<GetUsersResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = usersRepository
            .GetAllQueryable()
            .Select(u => new GetUsersResponse(u.Id.Value, u.Email.Value, u.UserName, u.FirstName, u.LastName))
            .ToList();

        var result = Result.Success(users);

        return Task.FromResult(result);
    }
}
