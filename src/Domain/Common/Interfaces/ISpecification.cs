using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Common.Interfaces;

public interface ISpecification<TEntity>
    where TEntity : Entity
{
    bool IsSatisfiedBy(TEntity entity);
}
