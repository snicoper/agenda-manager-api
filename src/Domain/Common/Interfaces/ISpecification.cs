namespace AgendaManager.Domain.Common.Interfaces;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}
