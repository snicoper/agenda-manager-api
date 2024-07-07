using AgendaManager.Application.Common.Models.Users;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface ICurrentUserProvider
{
    CurrentUser GetCurrentUser();
}
