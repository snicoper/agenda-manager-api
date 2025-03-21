using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Infrastructure.Common.Persistence;

namespace AgendaManager.TestCommon.Abstractions.Persistence;

public class DbContextWrapper(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}
