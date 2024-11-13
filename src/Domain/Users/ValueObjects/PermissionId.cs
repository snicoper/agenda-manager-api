namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record PermissionId
{
    private PermissionId(Guid value)
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
