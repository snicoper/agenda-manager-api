namespace AgendaManager.Domain.Common.Messaging.Interfaces;

public interface IOutboxMessageRepository
{
    Task<List<OutboxMessage>> GetPendingMessagesAsync(CancellationToken cancellationToken = default);
}
