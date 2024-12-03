using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Commands.AssignUserToRole;

public class AssignUserToRoleCommandValidator : AbstractValidator<AssignUserToRoleCommand>
{
    public AssignUserToRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.");

        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role Id is required.");
    }
}
