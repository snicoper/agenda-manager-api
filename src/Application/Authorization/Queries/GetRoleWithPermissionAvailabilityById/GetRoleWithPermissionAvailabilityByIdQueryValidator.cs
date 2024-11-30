using FluentValidation;

namespace AgendaManager.Application.Authorization.Queries.GetRoleWithPermissionAvailabilityById;

public class
    GetRoleWithPermissionAvailabilityByIdQueryValidator : AbstractValidator<GetRoleWithPermissionAvailabilityByIdQuery>
{
    public GetRoleWithPermissionAvailabilityByIdQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
