using System.Linq;
using Sample.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Sample.Infrastructure.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IAggregateRoot
    {
        private readonly OrdersDbContext _dbContext;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        protected abstract IQueryable<TEntity> AggregateQuery { get; }

        public IUnitOfWork UnitOfWork => _dbContext;

        public TEntity Add(TEntity entity) => DbSet.Add(entity).Entity;

        public TEntity Update(TEntity entity) => DbSet.Update(entity).Entity;

        public void Remove(TEntity entity) => DbSet.Remove(entity);
    }
}
