using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class UserTokenId : ValueObject
{
    private UserTokenId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static UserTokenId From(Guid value)
    {
        return new UserTokenId(value);
    }

    public static UserTokenId Create()
    {
        return new UserTokenId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
