namespace AgendaManager.TestCommon.Abstractions.Persistence;

public interface IDbContextWrapper
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
