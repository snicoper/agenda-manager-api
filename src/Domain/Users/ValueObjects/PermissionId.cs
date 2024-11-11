using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class PermissionId : ValueObject
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
