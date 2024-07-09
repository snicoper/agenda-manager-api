using FluentValidation;

namespace AgendaManager.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();

        RuleFor(r => r.Email).EmailAddress();
    }
}
