using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetAvailableRolesByUserId;

public class GetAvailableRolesByUserIdQueryValidator : AbstractValidator<GetAvailableRolesByUserIdQuery>
{
    public GetAvailableRolesByUserIdQueryValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
