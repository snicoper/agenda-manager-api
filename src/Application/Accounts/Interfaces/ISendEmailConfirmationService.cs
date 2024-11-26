using AgendaManager.Domain.Users;

namespace AgendaManager.Application.Accounts.Interfaces;

public interface ISendEmailConfirmationService
{
    Task SendAsync(User user, string token, CancellationToken cancellationToken = default);
}
