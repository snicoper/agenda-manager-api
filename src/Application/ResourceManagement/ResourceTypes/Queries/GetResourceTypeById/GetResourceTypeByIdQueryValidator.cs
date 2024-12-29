using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypeById;

public class GetResourceTypeByIdQueryValidator : AbstractValidator<GetResourceTypeByIdQuery>
{
    public GetResourceTypeByIdQueryValidator()
    {
        RuleFor(x => x.ResourceTypeId)
            .NotEmpty().WithMessage("ResourceTypeId is required.");
    }
}
