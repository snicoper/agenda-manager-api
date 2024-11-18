using AgendaManager.Application.Users.Models;

namespace AgendaManager.Application.Users.Services;

public interface ICurrentUserProvider
{
    public bool IsAuthenticated { get; }

    CurrentUser? GetCurrentUser();
}
