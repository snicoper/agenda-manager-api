using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountInfo;

public record GetAccountInfoQueryResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? PhoneNumberCountryCode,
    string? AddressStreet,
    string? AddressCity,
    string? AddressState,
    string? AddressCountry,
    string? AddressPostalCode,
    string? IdentityDocument,
    string? IdentityDocumentCountryCode,
    IdentityDocumentType? IdentityDocumentType,
    bool IsEmailConfirmed,
    bool IsActive,
    bool IsCollaborator);
