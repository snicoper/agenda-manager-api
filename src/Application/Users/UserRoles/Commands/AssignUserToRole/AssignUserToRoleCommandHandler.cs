using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.UserRoles.Commands.AssignUserToRole;

internal class AssignUserToRoleCommandHandler(AuthorizationService authorizationService, IUnitOfWork unitOfWork)
    : ICommandHandler<AssignUserToRoleCommand>
{
    public async Task<Result> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await authorizationService.AddRoleToUserAsync(
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
