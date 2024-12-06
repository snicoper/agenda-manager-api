using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.TestCommon.Constants;

namespace AgendaManager.TestCommon.Factories;

public static class UserProfileFactory
{
    public static UserProfile CreateUserProfile(
        UserProfileId? userProfileId = null,
        UserId? userId = null,
        string? firstName = null,
        string? lastName = null,
        PhoneNumber? phoneNumber = null,
        Address? address = null,
        IdentityDocument? identityDocument = null)
    {
        phoneNumber ??= PhoneNumber.From(
            number: "911882240",
            countryCode: "+34");

        address ??= Address.From(
            street: "Street",
            city: "City",
            state: "State",
            country: "Country",
            postalCode: "12345");

        identityDocument ??= IdentityDocument.From(
            value: "44123123Q",
            countryCode: "ES",
            type: IdentityDocumentType.NationalId);

        var userProfile = UserProfile.Create(
            userProfileId: userProfileId ?? UserProfileId.Create(),
            userId: userId ?? UserId.Create(),
            firstName: firstName ?? UserConstants.UserAlice.FirstName,
            lastName: lastName ?? UserConstants.UserAlice.LastName,
            phoneNumber: phoneNumber,
            address: address,
            identityDocument: identityDocument);

        return userProfile;
    }
}
