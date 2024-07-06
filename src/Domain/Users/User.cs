﻿using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
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

    public UserId Id { get; } = default!;

    public string UserName { get; } = default!;

    public Email Email { get; private set; } = default!;

    public string? FirstName { get; }

    public string? LastName { get; }

    public static User Create(UserId userId, Email email, string userName, string? firstName, string? lastName)
    {
        var user = new User(userId, email, userName, firstName, lastName);

        user.AddDomainEvent(new UserCreatedDomainEvent(userId));

        return user;
    }

    public User Update(Email email)
    {
        User userUpdated = new(Id, email, UserName, FirstName, LastName) { Created = Created, CreatedBy = CreatedBy };

        AddDomainEvent(new UserUpdatedDomainEvent(Id));

        return userUpdated;
    }
}
