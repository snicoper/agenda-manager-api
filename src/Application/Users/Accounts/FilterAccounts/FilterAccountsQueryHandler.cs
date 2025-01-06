using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Application.Users.Accounts.FilterAccounts;

internal class FilterAccountsQueryHandler(IUserRepository userRepository)
    : IQueryHandler<FilterAccountsQuery, List<FilterAccountsQueryResponse>>
{
    public async Task<Result<List<FilterAccountsQueryResponse>>> Handle(
        FilterAccountsQuery request,
        CancellationToken cancellationToken)
    {
        var users = await userRepository.GetQueryable()
            .Include(u => u.Profile)
            .Where(
                u => ((string)u.Email).Contains(request.Term)
                    || u.Profile.FirstName.Contains(request.Term)
                    || u.Profile.LastName.Contains(request.Term))
            .Select(
                u => new FilterAccountsQueryResponse(
                    u.Id.Value,
                    u.Email.Value,
                    u.Profile.FirstName,
                    u.Profile.LastName))
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return Result.Success(users);
    }
}
