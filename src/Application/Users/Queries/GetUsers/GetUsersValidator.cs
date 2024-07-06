using FluentValidation;

namespace AgendaManager.Application.Users.Queries.GetUsers;

public class GetUsersValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}
