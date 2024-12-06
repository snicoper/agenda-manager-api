using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
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

        user.Activate();

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
