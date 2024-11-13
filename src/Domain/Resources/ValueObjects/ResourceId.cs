﻿namespace AgendaManager.Domain.Resources.ValueObjects;

public sealed record ResourceId
{
    private ResourceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ResourceId From(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new ResourceId(value);
    }

    public static ResourceId Create()
    {
        return new ResourceId(Guid.NewGuid());
    }
}
