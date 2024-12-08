using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountsPaginated;

internal class GetAccountsPaginatedQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetAccountsPaginatedQuery, ResponseData<GetAccountsPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetAccountsPaginatedQueryResponse>>> Handle(
        GetAccountsPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        var users = userRepository.GetQueryable();
        users = UserFilter.ApplyFilters(users, request.RequestData);

        var responseData = await ResponseData<GetAccountsPaginatedQueryResponse>.CreateAsync(
            source: users,
            projection: u => new GetAccountsPaginatedQueryResponse(
                u.Id.Value,
                u.Email.Value,
                u.Profile.FirstName,
                u.Profile.LastName,
                u.IsActive,
                u.IsEmailConfirmed,
                u.IsCollaborator),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return responseData;
    }
}
