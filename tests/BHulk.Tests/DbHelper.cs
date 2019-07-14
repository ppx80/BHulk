using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sample.Domain.Domain.Order;
using Sample.Infrastructure.Data;

namespace BHulk.Tests
{
    internal static class DbHelper
    {
        internal static void UseConnection(Action<SqliteConnection> test)
        {
            using (var conn = new SqliteConnection("DataSource=:memory:"))
            {
                conn.Open();
                test(conn);
                conn.Close();
            }
        }

        internal static OrdersDbContext ContextFactory(SqliteConnection conn)
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseSqlite(conn)
                .EnableSensitiveDataLogging()
                .Options;
            var context = new OrdersDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        internal static async Task SeedData(OrdersDbContext context, int amount)
        {
            using (context)
            {
                for (var i = 0; i < amount; i++)
                {
                    var o = new Order((i + 1).ToString(), OrderStatus.Pending, OrderDetails.Empty());
                    context.Orders.Add(o);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
