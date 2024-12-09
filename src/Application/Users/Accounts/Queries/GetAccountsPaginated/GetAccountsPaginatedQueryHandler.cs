using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountsPaginated;

internal class GetAccountsPaginatedQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetAccountsPaginatedQuery, ResponseData<GetAccountsPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetAccountsPaginatedQueryResponse>>> Handle(
        GetAccountsPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        var users = userRepository.GetQueryable()
            .Include(u => u.Profile)
            .AsQueryable();

        users = UserFilter.ApplyFilters(users, request.RequestData);

        var responseData = await ResponseData<GetAccountsPaginatedQueryResponse>.CreateAsync(
            source: users,
            projection: u => new GetAccountsPaginatedQueryResponse(
                UserId: u.Id.Value,
                Email: u.Email.Value,
                FirstName: u.Profile.FirstName,
                LastName: u.Profile.LastName,
                IsActive: u.IsActive,
                IsEmailConfirmed: u.IsEmailConfirmed,
                IsCollaborator: u.IsCollaborator,
                DateJoined: u.CreatedAt),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
