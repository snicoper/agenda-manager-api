using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public class User : AuditableEntity
{
    private User(UserId userId, Email email, string userName)
    {
        Id = userId;
        UserName = userName;
        Email = email;
        EmailConfirmed = false;
    }

    private User()
    {
    }

    public UserId Id { get; } = default!;

    public string UserName { get; private set; } = default!;

    public Email Email { get; private set; } = default!;

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public bool EmailConfirmed { get; private set; }

    public bool Active { get; private set; } = true;

    public string? RefreshToken { get; private set; }

    public DateTimeOffset? RefreshTokenExpiryTime { get; private set; }

    public static User Create(UserId userId, Email email, string userName)
    {
        var user = new User(userId, email, userName);

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public void UpdateRefreshToken(string refreshToken, DateTimeOffset refreshTokenExpiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }
}
