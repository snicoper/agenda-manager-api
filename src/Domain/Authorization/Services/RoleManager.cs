using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Services;

public class RoleManager(IRoleRepository roleRepository)
{
    public async Task<Result<Role>> CreateRoleAsync(
        RoleId roleId,
        string name,
        string description,
        bool editable = false,
        CancellationToken cancellationToken = default)
    {
        Role role = new(roleId, name, description, editable);

        var validationResult = await IsValidAsync(role, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Role>();
        }

        await roleRepository.AddAsync(role, cancellationToken);

        return Result.Create(role);
    }

    public async Task<Result> UpdateRoleAsync(
        Role role,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        var validationResult = await IsValidAsync(role, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        role.Update(name, description);
        roleRepository.Update(role);

        return Result.Success();
    }

    private async Task<Result> IsValidAsync(Role role, CancellationToken cancellationToken)
    {
        if (await NameExistsAsync(role, cancellationToken))
        {
            return RoleErrors.RoleNameAlreadyExists;
        }

        return Result.Success();
    }

    private async Task<bool> NameExistsAsync(Role role, CancellationToken cancellationToken)
    {
        var nameIsUnique = await roleRepository.NameExistsAsync(role, cancellationToken);

        return nameIsUnique;
    }
}
