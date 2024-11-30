using FluentValidation;

namespace AgendaManager.Application.Authorization.Commands.UpdatePermissionForRole;

public class UpdatePermissionForRoleCommandValidator : AbstractValidator<UpdatePermissionForRoleCommand>
{
    public UpdatePermissionForRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.PermissionId)
            .NotEmpty();
    }
}
