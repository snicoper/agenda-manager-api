using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AuditableEntity
{
    private User()
    {
    }

    private User(UserId userId, Email email, string userName, string? firstName, string? lastName)
    {
        Id = userId;
        Email = email;
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
    }

    public UserId Id { get; private set; } = default!;

    public string UserName { get; private set; } = default!;

    public Email Email { get; private set; } = default!;

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public static User Create(UserId userId, Email email, string userName, string? firstName, string? lastName)
    {
        return new User(userId, email, userName, firstName, lastName);
    }
}
