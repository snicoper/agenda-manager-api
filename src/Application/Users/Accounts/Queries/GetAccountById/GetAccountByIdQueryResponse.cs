using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountById;

public record GetAccountByIdQueryResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    bool IsEmailConfirmed,
    bool IsActive,
    DateTimeOffset CreatedAt,
    GetAccountByIdQueryResponse.PhoneNumberResponse? PhoneNumber,
    GetAccountByIdQueryResponse.AddressResponse? Address,
    GetAccountByIdQueryResponse.IdentityDocumentResponse? IdentityDocument)
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
