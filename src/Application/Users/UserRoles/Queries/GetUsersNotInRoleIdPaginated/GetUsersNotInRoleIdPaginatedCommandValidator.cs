using FluentValidation;

namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleIdPaginated;

public class GetUsersNotInRoleIdPaginatedCommandValidator : AbstractValidator<GetUsersNotInRoleIdPaginatedCommand>
{
    public GetUsersNotInRoleIdPaginatedCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty()
            .WithMessage("Role Id is required.");
    }
}
