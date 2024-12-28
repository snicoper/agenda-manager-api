using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountDetails;

internal class GetAccountDetailsQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetAccountDetailsQuery, GetAccountDetailsQueryResponse>
{
    public async Task<Result<GetAccountDetailsQueryResponse>> Handle(
        GetAccountDetailsQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Get user by id and check if it exists.
        var user = await userRepository.GetByIdWithAllInfoAsync(UserId.From(request.UserId), cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // 2. Map user to response.
        var response = new GetAccountDetailsQueryResponse(
            UserId: user.Id.Value,
            Email: user.Email.Value,
            FirstName: user.Profile.FirstName,
            LastName: user.Profile.LastName,
            PhoneNumber: MapPhoneNumber(user.Profile.PhoneNumber),
            Address: MapAddress(user.Profile.Address),
            IdentityDocument: MapIdentityDocument(user.Profile.IdentityDocument),
            IsEmailConfirmed: user.IsEmailConfirmed,
            IsActive: user.IsActive,
            CreatedAt: user.CreatedAt);

        return response;
    }

    private static GetAccountDetailsQueryResponse.PhoneNumberResponse? MapPhoneNumber(PhoneNumber? phone)
    {
        var mapPhoneNumber = phone is not null
            ? new GetAccountDetailsQueryResponse.PhoneNumberResponse(
                Number: phone.Number,
                CountryCode: phone.CountryCode)
            : null;

        return mapPhoneNumber;
    }

    private static GetAccountDetailsQueryResponse.AddressResponse? MapAddress(Address? address)
    {
        var mapAddress = address is not null
            ? new GetAccountDetailsQueryResponse.AddressResponse(
                Street: address?.Street,
                City: address?.City,
                State: address?.State,
                Country: address?.Country,
                PostalCode: address?.PostalCode)
            : null;

        return mapAddress;
    }

    private static GetAccountDetailsQueryResponse.IdentityDocumentResponse? MapIdentityDocument(
        IdentityDocument? identityDocument)
    {
        var mapIdentityDocument = identityDocument is not null
            ? new GetAccountDetailsQueryResponse.IdentityDocumentResponse(
                Number: identityDocument?.Value,
                CountryCode: identityDocument?.CountryCode,
                Type: identityDocument?.Type)
            : null;

        return mapIdentityDocument;
    }
}
