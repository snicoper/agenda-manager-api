namespace AgendaManager.Domain.Common.Messaging.Interfaces;

public interface IOutboxMessageRepository
{
    Task<List<OutboxMessage>> GetMessagesForPublishAsync(CancellationToken cancellationToken = default);
}
