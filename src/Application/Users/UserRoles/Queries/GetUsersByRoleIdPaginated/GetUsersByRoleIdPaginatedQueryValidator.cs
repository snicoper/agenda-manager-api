using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersByRoleIdPaginated;

public class GetUsersByRoleIdPaginatedQueryValidator : AbstractValidator<GetUsersByRoleIdPaginatedQuery>
{
    public GetUsersByRoleIdPaginatedQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Id cannot be empty");
    }
}
