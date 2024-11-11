using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class RoleId : ValueObject
{
    private RoleId(Guid value)
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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
