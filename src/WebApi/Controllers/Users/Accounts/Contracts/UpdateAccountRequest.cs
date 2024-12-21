using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;

public record UpdateAccountRequest(
    string FirstName,
    string LastName,
    UpdateAccountRequest.PhoneRequest? Phone,
    UpdateAccountRequest.AddressRequest? Address,
    UpdateAccountRequest.IdentityDocumentRequest? IdentityDocument)
{
    public record PhoneRequest(string? Number, string? CountryCode);

    public record AddressRequest(string? Street, string? City, string? Country, string? State, string? PostalCode);

    public record IdentityDocumentRequest(string? Number, string? CountryCode, IdentityDocumentType? Type);
}
