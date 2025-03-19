using Newtonsoft.Json;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record UserProfileId
{
    [JsonConstructor]
    internal UserProfileId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static UserProfileId From(Guid value)
    {
        return new UserProfileId(value);
    }

    public static UserProfileId Create()
    {
        return new UserProfileId(Guid.NewGuid());
    }
}
