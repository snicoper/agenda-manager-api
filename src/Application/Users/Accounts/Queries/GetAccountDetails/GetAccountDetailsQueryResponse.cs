using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountDetails;

public record GetAccountDetailsQueryResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    bool IsEmailConfirmed,
    bool IsActive,
    bool IsCollaborator,
    DateTimeOffset CreatedAt,
    GetAccountDetailsQueryResponse.PhoneNumberResponse? PhoneNumber,
    GetAccountDetailsQueryResponse.AddressResponse? Address,
    GetAccountDetailsQueryResponse.IdentityDocumentResponse? IdentityDocument)
{
    public record PhoneNumberResponse(string? Number, string? CountryCode);

    public record AddressResponse(
        string? Street,
        string? City,
        string? State,
        string? Country,
        string? PostalCode);

    public record IdentityDocumentResponse(
        string? Number,
        string? CountryCode,
        IdentityDocumentType? Type);
}
