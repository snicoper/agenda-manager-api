using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Models;

public record CurrentUser(UserId UserId, IReadOnlyList<string> Roles, IReadOnlyList<string> Permissions);
