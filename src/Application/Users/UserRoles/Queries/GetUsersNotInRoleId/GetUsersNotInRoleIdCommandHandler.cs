using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleId;

internal class GetUsersNotInRoleIdCommandHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersNotInRoleIdCommand, ResponseData<GetUsersNotInRoleIdCommandResponse>>
{
    public async Task<Result<ResponseData<GetUsersNotInRoleIdCommandResponse>>> Handle(
        GetUsersNotInRoleIdCommand request,
        CancellationToken cancellationToken)
    {
        var users = userRepository.GetQueryableUsersNotInRoleId(RoleId.From(request.RoleId));

        users = UserFilter.ApplyFilters(users, request.RequestData);

        var responseData = await ResponseData<GetUsersNotInRoleIdCommandResponse>.CreateAsync(
            source: users,
            projection: u => new GetUsersNotInRoleIdCommandResponse(u.Id.Value, u.Email.Value),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
