using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Authorization.Services;

public class RoleManager(IRoleRepository roleRepository, IUserRepository userRepository)
{
    public async Task<Result<Role>> CreateRoleAsync(
        RoleId roleId,
        string name,
        string description,
        bool isEditable = false,
        CancellationToken cancellationToken = default)
    {
        Role role = new(roleId, name, description, isEditable);

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
        if (role.IsEditable is false)
        {
            return RoleErrors.RoleIsNotEditable;
        }

        role.Update(name, description);

        var validationResult = await IsValidAsync(role, cancellationToken);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        roleRepository.Update(role);

        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(RoleId roleId, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);
        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var roleHaveUsers = await userRepository.HasAnyUserWithRole(
            roleId,
            cancellationToken);

        if (roleHaveUsers)
        {
            return RoleErrors.RoleHasUsersAssigned;
        }

        roleRepository.Delete(role);

        return Result.Success();
    }

    private async Task<Result> IsValidAsync(Role role, CancellationToken cancellationToken)
    {
        if (await ExistsByNameAsync(role, cancellationToken))
        {
            return RoleErrors.RoleNameAlreadyExists;
        }

        return Result.Success();
    }

    private async Task<bool> ExistsByNameAsync(Role role, CancellationToken cancellationToken)
    {
        var nameIsUnique = await roleRepository.ExistsByNameAsync(role, cancellationToken);

        return nameIsUnique;
    }
}
