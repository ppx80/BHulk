namespace Sample.Domain.Domain
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<TEntity> : IRepository where TEntity : IAggregateRoot
    {
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
