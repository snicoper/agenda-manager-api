using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Authorization.Commands.UpdatePermissionForRole;

internal class UpdatePermissionForRoleCommandHandler(AuthorizationService authorizationService, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdatePermissionForRoleCommand>
{
    public async Task<Result> Handle(UpdatePermissionForRoleCommand request, CancellationToken cancellationToken)
    {
        // Add or remove permission from role.
        var result = request.IsAssigned
            ? await AddPermissionToRole(request.RoleId, request.PermissionId, cancellationToken)
            : await RemovePermissionFromRole(request.RoleId, request.PermissionId, cancellationToken);

        if (result.IsSuccess)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.NoContent();
    }

    private async Task<Result> AddPermissionToRole(Guid roleId, Guid permissionId, CancellationToken cancellationToken)
    {
        // Add permission to role.
        var result = await authorizationService.AddPermissionToRoleAsync(
            RoleId.From(roleId),
            PermissionId.From(permissionId),
            cancellationToken);

        return result;
    }

    private async Task<Result> RemovePermissionFromRole(
        Guid roleId,
        Guid permissionId,
        CancellationToken cancellationToken)
    {
        // Remove permission from role.
        var result = await authorizationService.RemovePermissionFromRoleAsync(
            RoleId.From(roleId),
            PermissionId.From(permissionId),
            cancellationToken);

        return result;
    }
}
