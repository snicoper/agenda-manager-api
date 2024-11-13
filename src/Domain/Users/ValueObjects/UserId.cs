﻿namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record UserId
{
    private UserId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static UserId From(Guid value)
    {
        return new UserId(value);
    }

    public static UserId Create()
    {
        return new UserId(Guid.NewGuid());
    }
}
