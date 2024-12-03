using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
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

        var responseData = await ResponseData<GetUsersNotInRoleIdCommandResponse>.CreateAsync(
            users,
            u => new GetUsersNotInRoleIdCommandResponse(u.Id.Value, u.Email.Value),
            request.RequestData,
            cancellationToken);

        return Result.Success(responseData);
    }
}
