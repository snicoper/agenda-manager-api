using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersByRoleId;

public class GetUsersByRoleIdQueryValidator : AbstractValidator<GetUsersByRoleIdQuery>
{
    public GetUsersByRoleIdQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Id cannot be empty");
    }
}
