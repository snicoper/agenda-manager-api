namespace AgendaManager.Domain.Common.Messaging.ValueObjects;

public sealed record OutboxMessageId
{
    private OutboxMessageId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static OutboxMessageId From(Guid value)
    {
        return new OutboxMessageId(value);
    }

    public static OutboxMessageId Create()
    {
        return new OutboxMessageId(Guid.NewGuid());
    }
}
