namespace AgendaManager.Domain.Common.Interfaces;

public interface ISpecification<TEntity>
    where TEntity : IEntity
{
    bool IsSatisfiedBy(TEntity entity);
}
