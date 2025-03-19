using Newtonsoft.Json;

namespace AgendaManager.Domain.Authorization.ValueObjects;

public sealed record PermissionId
{
    [JsonConstructor]
    internal PermissionId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static PermissionId From(Guid value)
    {
        return new PermissionId(value);
    }

    public static PermissionId Create()
    {
        return new PermissionId(Guid.NewGuid());
    }
}
