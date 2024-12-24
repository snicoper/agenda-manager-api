using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.UserRoles.Commands.UnAssignedUserFromRole;

internal class UnAssignedUserFromRoleCommandHandler(AuthorizationService authorizationService, IUnitOfWork unitOfWork)
    : ICommandHandler<UnAssignedUserFromRoleCommand>
{
    public async Task<Result> Handle(UnAssignedUserFromRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await authorizationService.RemoveRoleFromUserAsync(
            UserId.From(request.UserId),
            RoleId.From(request.RoleId),
            cancellationToken);

        if (result.IsSuccess)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.NoContent();
    }
}
