using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceId;

public class GetSchedulesByResourceIdQueryValidator : AbstractValidator<GetSchedulesByResourceIdQuery>
{
    public GetSchedulesByResourceIdQueryValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty().WithMessage("ResourceId is required.");
    }
}
