using AgendaManager.Application.Users.Models;

namespace AgendaManager.Application.Authentication.Interfaces;

public interface ICurrentUserProvider
{
    public bool IsAuthenticated { get; }

    CurrentUser? GetCurrentUser();
}
