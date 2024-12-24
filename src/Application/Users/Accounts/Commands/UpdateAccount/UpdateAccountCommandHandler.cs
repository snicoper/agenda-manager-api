using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.UpdateAccount;

internal class UpdateAccountCommandHandler(
    IUserRepository userRepository,
    UserProfileManager userProfileManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateAccountCommand>
{
    public async Task<Result> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        // 1. Get user by id and check if it exists.
        var user = await userRepository.GetByIdWithProfileAsync(UserId.From(request.UserId), cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // 2. Update user profile.
        var updateUserProfileResult = await userProfileManager.UpdateUserProfile(
            user: user,
            firstName: request.FirstName,
            lastName: request.LastName,
            phoneNumber: MapPhoneNumber(request.Phone),
            address: MapAddress(request.Address),
            identityDocument: MapIdentityDocument(request.IdentityDocument),
            cancellationToken: cancellationToken);

        if (updateUserProfileResult.IsFailure)
        {
            return updateUserProfileResult;
        }

        // 3. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 4. Return success.
        return Result.NoContent();
    }

    private static PhoneNumber? MapPhoneNumber(UpdateAccountCommand.PhoneCommand? phone)
    {
        var mapPhoneNumber = phone is not null
            && !string.IsNullOrEmpty(phone.Number)
            && !string.IsNullOrEmpty(phone.CountryCode)
                ? PhoneNumber.From(phone.Number, phone.CountryCode)
                : null;

        return mapPhoneNumber;
    }

    private static Address? MapAddress(UpdateAccountCommand.AddressCommand? address)
    {
        var mapAddress = address is not null
            && !string.IsNullOrEmpty(address.Street)
            && !string.IsNullOrEmpty(address.City)
            && !string.IsNullOrEmpty(address.Country)
            && !string.IsNullOrEmpty(address.State)
            && !string.IsNullOrEmpty(address.PostalCode)
                ? Address.From(
                    address.Street,
                    address.City,
                    address.Country,
                    address.State,
                    address.PostalCode)
                : null;

        return mapAddress;
    }

    private static IdentityDocument? MapIdentityDocument(UpdateAccountCommand.IdentityDocumentCommand? identityDocument)
    {
        var mapIdentityDocument = identityDocument is not null
            && !string.IsNullOrEmpty(identityDocument.Number)
            && !string.IsNullOrEmpty(identityDocument.CountryCode)
            && identityDocument.Type is not null
                ? IdentityDocument.From(
                    identityDocument.Number,
                    identityDocument.CountryCode,
                    identityDocument.Type.Value)
                : null;

        return mapIdentityDocument;
    }
}
