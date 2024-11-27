using AgendaManager.Domain.Users;

namespace AgendaManager.Application.Users.Interfaces;

public interface ISendRecoveryPasswordService
{
    Task SendAsync(User user, string token, CancellationToken cancellationToken = default);
}
