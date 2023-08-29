namespace HexagonalArch.Domain.SeedWork;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
