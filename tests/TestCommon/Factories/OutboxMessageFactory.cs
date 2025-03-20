using AgendaManager.Domain.Common.Messaging;
using AgendaManager.Domain.Common.Messaging.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class OutboxMessageFactory
{
    public static OutboxMessage CreateOutboxMessage(
        OutboxMessageId? id = null,
        string? type = null,
        string? payload = null)
    {
        var outboxMessage = OutboxMessage.Create(
            outboxMessageId: id ?? OutboxMessageId.Create(),
            type: type ?? "test",
            payload: payload ?? "test");

        return outboxMessage;
    }
}
