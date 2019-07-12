using System.Linq;
using Sample.Domain.Domain.Order;

namespace Sample.Infrastructure.Data.Repositories
{
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(OrdersDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Order> AggregateQuery => DbSet;

    }
}
