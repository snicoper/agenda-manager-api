using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Models;

public record CurrentUser(UserId Id, IReadOnlyList<string> Roles, IReadOnlyList<string> Permissions);
