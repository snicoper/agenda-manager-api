using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceIdPaginated;

public class GetSchedulesByResourceIdPaginatedQueryValidator : AbstractValidator<GetSchedulesByResourceIdPaginatedQuery>
{
    public GetSchedulesByResourceIdPaginatedQueryValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty().WithMessage("ResourceId is required.");
    }
}
