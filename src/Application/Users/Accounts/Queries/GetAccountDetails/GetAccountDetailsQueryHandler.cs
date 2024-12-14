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
        var user = await userRepository.GetAllInfoByIdAsync(UserId.From(request.UserId), cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        var response = new GetAccountDetailsQueryResponse(
            UserId: user.Id.Value,
            Email: user.Email.Value,
            FirstName: user.Profile.FirstName,
            LastName: user.Profile.LastName,
            PhoneNumber: user.Profile.PhoneNumber?.Number,
            PhoneNumberCountryCode: user.Profile.PhoneNumber?.CountryCode,
            AddressStreet: user.Profile.Address?.Street,
            AddressCity: user.Profile.Address?.City,
            AddressState: user.Profile.Address?.State,
            AddressCountry: user.Profile.Address?.Country,
            AddressPostalCode: user.Profile.Address?.PostalCode,
            IdentityDocument: user.Profile.IdentityDocument?.Value,
            IdentityDocumentCountryCode: user.Profile.IdentityDocument?.CountryCode,
            IdentityDocumentType: user.Profile.IdentityDocument?.Type,
            IsEmailConfirmed: user.IsEmailConfirmed,
            IsActive: user.IsActive,
            IsCollaborator: user.IsCollaborator,
            CreatedAt: user.CreatedAt);

        return response;
    }
}
