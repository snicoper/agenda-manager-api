using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetResourceById;

public class GetResourceByIdQueryValidator : AbstractValidator<GetResourceByIdQuery>
{
    public GetResourceByIdQueryValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty().WithMessage("ResourceId is required.");
    }
}
