using AgendaManager.Domain.Common.Messaging.Enums;
using AgendaManager.Domain.Common.Messaging.Exceptions;
using AgendaManager.Domain.Common.Messaging.ValueObjects;

namespace AgendaManager.Domain.Common.Messaging;

public sealed class OutboxMessage
{
    private OutboxMessage(OutboxMessageId outboxMessageId, string type, string payload)
    {
        GuardAgainstInvalidType(type);
        GuardAgainstInvalidPayload(payload);

        Id = outboxMessageId;
        Type = type;
        Payload = payload;
        OccurredOn = DateTimeOffset.UtcNow;
        MessageStatus = OutboxMessageStatus.Pending;
    }

    private OutboxMessage()
    {
    }

    public OutboxMessageId Id { get; } = null!;

    public DateTimeOffset OccurredOn { get; private set; }

    public string Type { get; } = null!;

    public string Payload { get; private set; } = null!;

    public OutboxMessageStatus MessageStatus { get; private set; }

    public DateTimeOffset? ProcessedOn { get; private set; }

    public string? Error { get; private set; }

    public static OutboxMessage Create(OutboxMessageId outboxMessageId, string type, string payload)
    {
        OutboxMessage outboxMessage = new(outboxMessageId, type, payload);

        return outboxMessage;
    }

    public void MarkAsProcessed()
    {
        MessageStatus = OutboxMessageStatus.Processed;
        ProcessedOn = DateTimeOffset.UtcNow;
    }

    public void MarkAsFailed(string error)
    {
        MessageStatus = OutboxMessageStatus.Failed;
        Error = error;
    }

    private static void GuardAgainstInvalidType(string type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (string.IsNullOrWhiteSpace(type))
        {
            throw new OutboxMessageDomainException("Type cannot be empty.");
        }
    }

    private static void GuardAgainstInvalidPayload(string payload)
    {
        ArgumentNullException.ThrowIfNull(payload);

        if (string.IsNullOrWhiteSpace(payload))
        {
            throw new OutboxMessageDomainException("Payload cannot be empty.");
        }
    }
}
