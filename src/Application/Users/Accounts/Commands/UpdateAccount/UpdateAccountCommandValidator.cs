using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        // Validación de Phone.
        When(
            x => !string.IsNullOrEmpty(x.Phone?.Number) || !string.IsNullOrEmpty(x.Phone?.CountryCode),
            () =>
            {
                RuleFor(x => x.Phone!.Number)
                    .NotEmpty().WithMessage("Phone number is required when phone is provided.")
                    .MaximumLength(15).WithMessage("Phone number must not exceed 15 characters.");

                RuleFor(x => x.Phone!.Number)
                    .NotEmpty().WithMessage("Phone number is required when phone is provided.");
            });

        // Validación de Address
        When(
            x => !string.IsNullOrEmpty(x.Address?.Street) ||
                 !string.IsNullOrEmpty(x.Address?.City) ||
                 !string.IsNullOrEmpty(x.Address?.Country) ||
                 !string.IsNullOrEmpty(x.Address?.State) ||
                 !string.IsNullOrEmpty(x.Address?.PostalCode),
            () =>
            {
                RuleFor(x => x.Address!.Street)
                    .NotEmpty().WithMessage("Street is required when address is provided.");

                RuleFor(x => x.Address!.City)
                    .NotEmpty().WithMessage("City is required when address is provided.");

                RuleFor(x => x.Address!.Country)
                    .NotEmpty().WithMessage("Country is required when address is provided.");

                RuleFor(x => x.Address!.State)
                    .NotEmpty().WithMessage("State is required when address is provided.");

                RuleFor(x => x.Address!.PostalCode)
                    .NotEmpty().WithMessage("Postal code is required when address is provided.");
            });

        // Validación de IdentityDocument.
        When(
            x => !string.IsNullOrEmpty(x.IdentityDocument?.Number) ||
                 !string.IsNullOrEmpty(x.IdentityDocument?.CountryCode) ||
                 x.IdentityDocument?.Type is not null,
            () =>
            {
                RuleFor(x => x.IdentityDocument!.Number)
                    .NotEmpty().WithMessage("Identity document number is required when identity document is provided.");

                RuleFor(x => x.IdentityDocument!.CountryCode)
                    .NotEmpty().WithMessage(
                        "Identity document country code is required when identity document is provided.");

                RuleFor(x => x.IdentityDocument!.Type)
                    .NotEmpty().WithMessage("Identity document type is required when identity document is provided.");
            });
    }
}
