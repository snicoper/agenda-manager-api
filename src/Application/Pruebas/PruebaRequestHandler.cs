using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Common.ValueObjects.Token;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using MediatR;

namespace AgendaManager.Application.Pruebas;

internal class PruebaRequestHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    : IQueryHandler<PruebaRequest, Unit>
{
    public async Task<Result<Unit>> Handle(PruebaRequest request, CancellationToken cancellationToken)
    {
        var email = EmailAddress.From("alice@example.com");

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        var userToken = UserToken.Create(
            id: UserTokenId.Create(),
            userId: user.Id,
            token: Token.Generate(TimeSpan.FromDays(1)),
            type: UserTokenType.EmailConfirmation);

        user.AddUserToken(userToken);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
