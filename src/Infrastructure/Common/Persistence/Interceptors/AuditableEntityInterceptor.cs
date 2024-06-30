using AgendaManager.Application.Common.Abstractions.Clock;
using AgendaManager.Application.Common.Abstractions.Users;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Infrastructure.Common.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AgendaManager.Infrastructure.Common.Persistence.Interceptors;

public class AuditableEntityInterceptor(ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (EntityEntry<IAuditableEntity> entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedBy = currentUserService.Id;
                entry.Entity.Created = dateTimeProvider.UtcNow;
            }

            if (entry.State != EntityState.Added && entry.State != EntityState.Modified && !entry.HasChangedOwnedEntities())
            {
                continue;
            }

            entry.Entity.LastModifiedBy = currentUserService.Id;
            entry.Entity.LastModified = dateTimeProvider.UtcNow;
        }
    }
}
