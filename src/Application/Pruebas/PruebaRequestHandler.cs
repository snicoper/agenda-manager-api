using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Interfaces;
using MediatR;

namespace AgendaManager.Application.Pruebas;

internal class PruebaRequestHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
    : IQueryHandler<PruebaRequest, Unit>
{
    public async Task<Result<Unit>> Handle(PruebaRequest request, CancellationToken cancellationToken)
    {
        var email = EmailAddress.From("alice@example.com");

        // Contexto 1
        var user1 = await userRepository.GetByEmailAsync(email, cancellationToken);
        user1!.Version = 12222;

        userRepository.Update(user1!);
        await unitOfWork.SaveChangesAsync(cancellationToken); // Cambia `RowVersion`

        return Unit.Value;
    }
}
