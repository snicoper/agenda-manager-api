using FluentValidation;

namespace AgendaManager.Application.Authorization.Queries.GetRolePermissionsById;

public class GetRolePermissionsByIdQueryValidator : AbstractValidator<GetRolePermissionsByIdQuery>
{
    public GetRolePermissionsByIdQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
