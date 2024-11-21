using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Services;

public class PermissionManager(IPermissionRepository permissionRepository)
{
    public async Task<Result<Permission>> CreatePermissionAsync(
        PermissionId permissionId,
        string name,
        CancellationToken cancellationToken = default)
    {
        Permission permission = new(permissionId, name);

        var validationResult = await IsValidAsync(permission, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Permission>();
        }

        await permissionRepository.AddAsync(permission, cancellationToken);

        return Result.Create(permission);
    }

    private async Task<Result> IsValidAsync(Permission permission, CancellationToken cancellationToken)
    {
        if (await ExistsByNameAsync(permission, cancellationToken))
        {
            return PermissionErrors.PermissionNameAlreadyExists;
        }

        return Result.Success();
    }

    private async Task<bool> ExistsByNameAsync(Permission permission, CancellationToken cancellationToken)
    {
        var nameIsUnique = await permissionRepository.ExistsByNameAsync(permission, cancellationToken);

        return nameIsUnique;
    }
}
