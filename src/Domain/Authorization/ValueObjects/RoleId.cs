﻿using Newtonsoft.Json;

namespace AgendaManager.Domain.Authorization.ValueObjects;

public sealed record RoleId
{
    [JsonConstructor]
    internal RoleId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static RoleId From(Guid value)
    {
        return new RoleId(value);
    }

    public static RoleId Create()
    {
        return new RoleId(Guid.NewGuid());
    }
}
