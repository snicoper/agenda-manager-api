using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Aggregates;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

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
        if (await NameExistsAsync(permission, cancellationToken))
        {
            return PermissionErrors.PermissionNameAlreadyExists;
        }

        return Result.Success();
    }

    private async Task<bool> NameExistsAsync(Permission permission, CancellationToken cancellationToken)
    {
        var nameIsUnique = await permissionRepository.NameExistsAsync(permission, cancellationToken);

        return nameIsUnique;
    }
}
