﻿using Newtonsoft.Json;

namespace AgendaManager.Domain.AuditRecords.ValueObjects;

public sealed record AuditRecordId
{
    [JsonConstructor]
    internal AuditRecordId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static AuditRecordId From(Guid value)
    {
        return new AuditRecordId(value);
    }

    public static AuditRecordId Create()
    {
        return new AuditRecordId(Guid.NewGuid());
    }
}
