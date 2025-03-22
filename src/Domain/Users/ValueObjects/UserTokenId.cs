using Newtonsoft.Json;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record UserTokenId
{
    [JsonConstructor]
    internal UserTokenId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

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
}
