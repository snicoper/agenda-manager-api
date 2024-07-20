using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AgendaManager.Infrastructure.Common.Persistence.Interceptors;

public class AuditableEntityInterceptor(ICurrentUserProvider currentUserProvider, IDateTimeProvider dateTimeProvider)
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

        var currentDateTime = dateTimeProvider.UtcNow;
        var currentUser = currentUserProvider.GetCurrentUser();
        var currentUserId = currentUser?.Id.Value.ToString() ?? "System";

        const string createdBy = nameof(IAuditableEntity.CreatedBy);
        const string createdAt = nameof(IAuditableEntity.CreatedAt);
        const string lastModifiedBy = nameof(IAuditableEntity.LastModifiedBy);
        const string lastModifiedAt = nameof(IAuditableEntity.LastModifiedAt);

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State is EntityState.Added)
            {
                if (entry.Metadata.FindProperty(createdBy) is not null)
                {
                    entry.Property(createdBy).CurrentValue = currentUserId;
                }

                if (entry.Metadata.FindProperty(createdAt) is not null)
                {
                    entry.Property(createdAt).CurrentValue = currentDateTime;
                }
            }

            if (entry.State != EntityState.Added && entry.State != EntityState.Modified &&
                !entry.HasChangedOwnedEntities())
            {
                continue;
            }

            if (entry.Metadata.FindProperty(lastModifiedBy) is not null)
            {
                entry.Property(lastModifiedBy).CurrentValue = currentUserId;
            }

            if (entry.Metadata.FindProperty(lastModifiedAt) is not null)
            {
                entry.Property(lastModifiedAt).CurrentValue = currentDateTime;
            }
        }
    }
}
