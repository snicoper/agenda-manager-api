using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleId;

public class GetUsersNotInRoleIdCommandValidator : AbstractValidator<GetUsersNotInRoleIdCommand>
{
    public GetUsersNotInRoleIdCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role Id is required.");
    }
}
