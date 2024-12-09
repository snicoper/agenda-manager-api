using AgendaManager.Domain.Users;

namespace AgendaManager.Application.Users.Interfaces;

public interface ISendAccountConfirmationService
{
    Task SendAsync(User user, string token, CancellationToken cancellationToken = default);
}
