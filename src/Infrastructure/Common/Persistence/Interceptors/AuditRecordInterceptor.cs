using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Persistence.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AgendaManager.Infrastructure.Common.Persistence.Interceptors;

public class AuditRecordInterceptor(AuditRecordInterceptorService auditRecordInterceptorService)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // Users.
        auditRecordInterceptorService.UpdateEntities<User>(
            context: eventData.Context,
            entityId: nameof(User.Id),
            propertyNames: [nameof(User.Active)]);

        // Roles.
        auditRecordInterceptorService.UpdateEntities<Role>(
            context: eventData.Context,
            entityId: nameof(Role.Id),
            propertyNames: [nameof(Role.Editable)]);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        // Users.
        auditRecordInterceptorService.UpdateEntities<User>(
            context: eventData.Context,
            entityId: nameof(User.Id),
            propertyNames: [nameof(User.Active)]);

        // Roles.
        auditRecordInterceptorService.UpdateEntities<Role>(
            eventData.Context,
            nameof(Role.Id),
            [nameof(Role.Editable)]);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
