using AgendaManager.Application.Common.Interfaces.Clock;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    private static void UpdateAuditableModifiedFields(
        EntityEntry entry,
        string currentUserId,
        DateTimeOffset currentDateTime)
    {
        const string fieldLastModifiedBy = nameof(IAuditableEntity.LastModifiedBy);
        const string fieldLastModifiedAt = nameof(IAuditableEntity.LastModifiedAt);

        if (entry.Metadata.FindProperty(fieldLastModifiedBy) is not null)
        {
            entry.Property(fieldLastModifiedBy).CurrentValue = currentUserId;
        }

        if (entry.Metadata.FindProperty(fieldLastModifiedAt) is not null)
        {
            entry.Property(fieldLastModifiedAt).CurrentValue = currentDateTime;
        }
    }

    private static void UpdateAuditableCreateFields(
        EntityEntry entry,
        string currentUserId,
        DateTimeOffset currentDateTime)
    {
        const string fieldCreatedBy = nameof(IAuditableEntity.CreatedBy);
        const string fieldCreatedAt = nameof(IAuditableEntity.CreatedAt);

        if (entry.Metadata.FindProperty(fieldCreatedBy) is not null)
        {
            entry.Property(fieldCreatedBy).CurrentValue = currentUserId;
        }

        if (entry.Metadata.FindProperty(fieldCreatedAt) is not null)
        {
            entry.Property(fieldCreatedAt).CurrentValue = currentDateTime;
        }
    }

    private static void UpdateConcurrencyVersion(EntityEntry? entry)
    {
        if (entry is null)
        {
            return;
        }

        const string fieldVersion = nameof(IAuditableEntity.Version);
        var versionProperty = entry.Metadata.FindProperty(fieldVersion);

        if (versionProperty is null)
        {
            return;
        }

        var versionPropertyEntry = entry.Property(fieldVersion);
        if (versionPropertyEntry.CurrentValue is null || versionPropertyEntry.OriginalValue is null)
        {
            return;
        }

        // Comparar versiones y lanzar excepción si no coinciden.
        var currentValue = (int)versionPropertyEntry.CurrentValue;
        var originalValue = (int)versionPropertyEntry.OriginalValue;

        if (currentValue != 0 && currentValue != originalValue)
        {
            throw new DbUpdateConcurrencyException(
                "Concurrency conflict detected, the entity has been modified by another user.");
        }

        var newVersion = currentValue + 1;

        versionPropertyEntry.CurrentValue = newVersion;
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

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State is EntityState.Added)
            {
                UpdateAuditableCreateFields(entry, currentUserId, currentDateTime);
            }

            if (entry.State != EntityState.Added && entry.State != EntityState.Modified &&
                !entry.HasChangedOwnedEntities())
            {
                continue;
            }

            UpdateAuditableModifiedFields(entry, currentUserId, currentDateTime);

            UpdateConcurrencyVersion(entry);
        }
    }
}
