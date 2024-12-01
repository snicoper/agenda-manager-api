using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Authorization.Interfaces;

public interface IAuthorizationCheckService
{
    bool HasPermissionOrIsOwner(string permission, UserId ownerId);
}
