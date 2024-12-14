using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountDetails;

public record GetAccountDetailsQueryResponse(
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
    bool IsCollaborator,
    DateTimeOffset CreatedAt);
