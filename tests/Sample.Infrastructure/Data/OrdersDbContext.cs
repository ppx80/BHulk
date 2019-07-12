using Sample.Domain.Domain;
using Sample.Domain.Domain.Order;
using Microsoft.EntityFrameworkCore;

namespace Sample.Infrastructure.Data
{
    public class OrdersDbContext : DbContext, IUnitOfWork
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        { }

        public DbSet<Order> Orders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
        }
    }
}
