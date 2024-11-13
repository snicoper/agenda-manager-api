namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record UserTokenId
{
    private UserTokenId(Guid value)
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
