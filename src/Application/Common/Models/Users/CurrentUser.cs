using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Common.Models.Users;

public record CurrentUser(UserId Id, IReadOnlyList<string> Roles, IReadOnlyList<string> Permissions);
