using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class RoleService(IRoleRepository roleRepository)
{
    public async Task<Result<Role>> CreateAsync(
        RoleId roleId,
        string name,
        bool editable = false,
        CancellationToken cancellationToken = default)
    {
        Role role = new(roleId, name, editable);

        var validationResult = await IsValidAsync(role, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<Role>();
        }

        await roleRepository.AddAsync(role, cancellationToken);

        return Result.Create(role);
    }

    public async Task<Result> UpdateAsync(Role role, CancellationToken cancellationToken)
    {
        var validationResult = await IsValidAsync(role, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        roleRepository.Update(role);

        return Result.Success();
    }

    public async Task<Result> IsValidAsync(Role role, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(role.Name) || role.Name.Length > 100)
        {
            return RoleErrors.RoleNameExceedsLength;
        }

        if (await NameExistsAsync(role, cancellationToken))
        {
            return RoleErrors.RoleNameAlreadyExists;
        }

        return Result.Success();
    }

    public async Task<bool> NameExistsAsync(Role role, CancellationToken cancellationToken)
    {
        var nameIsUnique = await roleRepository.NameExistsAsync(role, cancellationToken);

        return nameIsUnique;
    }
}
