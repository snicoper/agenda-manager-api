using FluentValidation;

namespace AgendaManager.Application.Users.Queries.GetUsers;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
    }
}
