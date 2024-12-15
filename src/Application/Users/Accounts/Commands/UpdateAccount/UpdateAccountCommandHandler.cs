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
        var user = await userRepository.GetByIdAsync(UserId.From(request.UserId), cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // 2. Update user profile.
        var updateUserProfileResult = await userProfileManager.UpdateUserProfile(
            user,
            request.FirstName,
            request.LastName,
            PhoneNumber.From(request.Phone.Number, request.Phone.CountryCode),
            Address.From(
                request.Address.Street,
                request.Address.City,
                request.Address.Country,
                request.Address.State,
                request.Address.PostalCode),
            IdentityDocument.From(
                request.IdentityDocument.Number,
                request.IdentityDocument.CountryCode,
                request.IdentityDocument.Type),
            cancellationToken);

        if (updateUserProfileResult.IsFailure)
        {
            return updateUserProfileResult;
        }

        // 3. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 4. Return success.
        return Result.Success();
    }
}
