using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public sealed class UserProfileManager(IUserProfileRepository userProfileRepository, IUserRepository userRepository)
{
    public async Task<Result> UpdateUserProfile(
        User user,
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber,
        Address? address,
        IdentityDocument? identityDocument,
        CancellationToken cancellationToken)
    {
        // 1. Check if the user has changes.
        if (!user.Profile.HasChanges(firstName, lastName, phoneNumber, address, identityDocument))
        {
            return Result.Success();
        }

        // 2. Check uniqueness of IdentityDocument if exists.
        if (identityDocument is not null
            && user.Profile.IdentityDocument != identityDocument
            && await IdentityDocumentExistsAsync(identityDocument, cancellationToken))
        {
            return UserProfileErrors.IdentityDocumentAlreadyExists;
        }

        // 3. Update user profile.
        user.UpdateProfile(firstName, lastName, phoneNumber, address, identityDocument);

        // 4. Save changes.
        userRepository.Update(user);

        return Result.Success();
    }

    private async Task<bool> IdentityDocumentExistsAsync(
        IdentityDocument identityDocument,
        CancellationToken cancellationToken)
    {
        var identityDocumentExists = await userProfileRepository.IdentityDocumentExistsAsync(
            identityDocument,
            cancellationToken);

        return identityDocumentExists;
    }
}
