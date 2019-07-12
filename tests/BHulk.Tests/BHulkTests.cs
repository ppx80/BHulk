using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Sample.Domain.Domain.Order;
using Sample.Infrastructure.Data;
using Xunit;

namespace BHulk.Tests
{
    public class BHulkTests
    {
        [Fact]
        public void Update_ValidValues_DataShouldBeUpdated()
        {
            var indexes = new long[] { 1, 2, 3, 4, 5 };

            UseConnection(async(conn) =>
            {
                await SeedData(ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => ContextFactory(conn))
                    .Set(o => o.OrderCode, "test")
                    .Set(o => o.Status, OrderStatus.Executed)
                    .Set(o => o.ModifiedDate, DateTime.UtcNow)
                    .For(indexes)
                    .InStepOf(2);

                var sql = await update.ExecuteAsync();

                sql.Should().Be(5);
            });
        }

        [Fact]
        public void UpdateByPredicate_ValidValues_DataShouldBeUpdated()
        {
            UseConnection(async (conn) =>
            {
                await SeedData(ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => ContextFactory(conn))
                    .Set(o => o.OrderCode, "test")
                    .Set(o => o.Status, OrderStatus.Executed)
                    .Set(o => o.ModifiedDate, DateTime.UtcNow)
                    .For(i => i.Status == OrderStatus.Pending)
                    .InStepOf(2);

                var sql = await update.ExecuteAsync();

                sql.Should().Be(10);
            });
        }

        [Fact]
        public void Update_UpdateModifiedDate_DateShouldBeUpdated()
        {
            var indexes = new long[] { 1 };
            var date = DateTime.UtcNow;

            UseConnection(async (conn) =>
            {
                await SeedData(ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => ContextFactory(conn))
                    .Set(o => o.ModifiedDate, date)
                    .For(indexes)
                    .InStepOf(2);

                await update.ExecuteAsync();
                var updatedOrder = ContextFactory(conn).Orders.First(o => o.Id == 1);
                updatedOrder.ModifiedDate.Should().Be(date);
            });
        }

        [Fact]
        public void AddSetter_InvalidValue_ShouldThrowException()
        {
            var indexes = new List<long>() {1, 2, 3, 4};

            UseConnection(conn =>
            {
                Action act = () =>
                {
                    BHulk<Order>
                        .UseContext(() => ContextFactory(conn))
                        .Set(o => o.OrderCode, "test")
                        .Set(o => o.ModifiedDate, "test")
                        .For(indexes)
                        .InStepOf(2);
                };

                act.Should().Throw<ArgumentException>()
                    .Where(e => e.Message.StartsWith("Invalid value for ModifiedDate"));
            });
        }


        private static void UseConnection(Action<SqliteConnection> test)
        {
            using (var conn = new SqliteConnection("DataSource=:memory:"))
            {
                conn.Open();
                test(conn);
                conn.Close();
            }
        }

        private static OrdersDbContext ContextFactory(SqliteConnection conn)
        {
            var options = new DbContextOptionsBuilder<OrdersDbContext>()
                .UseSqlite(conn)
                .EnableSensitiveDataLogging()
                .Options;
            var context = new OrdersDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private static async Task SeedData(OrdersDbContext context, int amount)
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
