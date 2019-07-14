using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sample.Domain.Domain.Order;
using Xunit;

namespace BHulk.Tests
{
    public class BHulkTests
    {
        [Fact]
        public void UpdateAsync_ValidValues_DataShouldBeUpdated()
        {
            var indexes = new long[] { 1, 2, 3, 4, 5 };

            DbHelper.UseConnection(async(conn) =>
            {
                await DbHelper.SeedData(DbHelper.ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => DbHelper.ContextFactory(conn))
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
        public void UpdateAsync_ValidValuesForOwnedType_DataShouldBeUpdated()
        {
            var indexes = new long[] { 1, 2, 3, 4, 5 };

            DbHelper.UseConnection(async (conn) =>
            {
                await DbHelper.SeedData(DbHelper.ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => DbHelper.ContextFactory(conn))
                    .Set("OrderDetails_TotalAmount", 10m)
                    .For(indexes)
                    .InStepOf(2);

                var sql = await update.ExecuteAsync();
                var updatedOrder = DbHelper.ContextFactory(conn).Orders.First(o => o.Id == 1);
                updatedOrder.OrderDetails.TotalAmount.Should().Be(10m);
                sql.Should().Be(5);
            });
        }

        [Fact]
        public void Update_ValidValues_DataShouldBeUpdated()
        {
            var indexes = new long[] { 1, 2, 3, 4, 5 };

            DbHelper.UseConnection(async (conn) =>
            {
                await DbHelper.SeedData(DbHelper.ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => DbHelper.ContextFactory(conn))
                    .Set(o => o.OrderCode, "test")
                    .Set(o => o.Status, OrderStatus.Executed)
                    .Set(o => o.ModifiedDate, DateTime.UtcNow)
                    .For(indexes)
                    .InStepOf(2);

                var sql = update.Execute();

                sql.Should().Be(5);
            });
        }

        [Fact]
        public void UpdateByPredicate_ValidValues_DataShouldBeUpdated()
        {
            DbHelper.UseConnection(async (conn) =>
            {
                await DbHelper.SeedData(DbHelper.ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => DbHelper.ContextFactory(conn))
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

            DbHelper.UseConnection(async (conn) =>
            {
                await DbHelper.SeedData(DbHelper.ContextFactory(conn), 10);

                var update = BHulk<Order>
                    .UseContext(() => DbHelper.ContextFactory(conn))
                    .Set(o => o.ModifiedDate, date)
                    .For(indexes)
                    .InStepOf(2);

                await update.ExecuteAsync();
                var updatedOrder = DbHelper.ContextFactory(conn).Orders.First(o => o.Id == 1);
                updatedOrder.ModifiedDate.Should().Be(date);
            });
        }

        [Fact]
        public void AddSetter_InvalidValue_ShouldThrowException()
        {
            var indexes = new List<long>() {1, 2, 3, 4};

            DbHelper.UseConnection(conn =>
            {
                Action act = () =>
                {
                    BHulk<Order>
                        .UseContext(() => DbHelper.ContextFactory(conn))
                        .Set(o => o.OrderCode, "test")
                        .Set(o => o.ModifiedDate, "test")
                        .For(indexes)
                        .InStepOf(2);
                };

                act.Should().Throw<ArgumentException>()
                    .Where(e => e.Message.StartsWith("Invalid value for ModifiedDate"));
            });
        }
    }
}
