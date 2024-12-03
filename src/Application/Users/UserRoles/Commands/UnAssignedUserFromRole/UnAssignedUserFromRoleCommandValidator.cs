using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Commands.UnAssignedUserFromRole;

public class UnAssignedUserFromRoleCommandValidator : AbstractValidator<UnAssignedUserFromRoleCommand>
{
    public UnAssignedUserFromRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role Id is required.");
    }
}
