using Sample.Domain.Domain.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.Infrastructure.Data.EntityConfigurations
{
    public class EntityTypeOrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            builder.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
        }
    }
}
