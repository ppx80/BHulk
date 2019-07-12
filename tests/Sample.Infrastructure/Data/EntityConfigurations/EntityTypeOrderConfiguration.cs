using Sample.Domain.Domain.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.Infrastructure.Data.EntityConfigurations
{
    public class EntityTypeOrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(r => r.OrderDetails, od =>
            {
                od.OwnsOne(i => i.BillingAddress);
                od.OwnsOne(i => i.ShippingAddress);
            });

            builder.HasIndex(p => p.Id);

            builder.Property(p => p.OrderCode).IsRequired().HasMaxLength(10);

        }
    }
}
