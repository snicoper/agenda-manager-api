using AgendaManager.Domain.Common.Messaging;
using AgendaManager.Domain.Common.Messaging.Enums;
using AgendaManager.Domain.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Common.Messaging.Repositories;

public class OutboxMessageRepository(AppDbContext context) : IOutboxMessageRepository
{
    public async Task<List<OutboxMessage>> GetPendingMessagesAsync(CancellationToken cancellationToken = default)
    {
        var messages = await context.OutboxMessages
            .Where(x => x.MessageStatus == OutboxMessageStatus.Pending)
            .OrderBy(x => x.OccurredOn)
            .Take(20)
            .ToListAsync(cancellationToken);

        return messages;
    }
}
