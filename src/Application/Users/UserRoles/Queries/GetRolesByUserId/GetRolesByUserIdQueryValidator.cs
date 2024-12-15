using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetRolesByUserId;

public class GetRolesByUserIdQueryValidator : AbstractValidator<GetRolesByUserIdQuery>
{
    public GetRolesByUserIdQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
