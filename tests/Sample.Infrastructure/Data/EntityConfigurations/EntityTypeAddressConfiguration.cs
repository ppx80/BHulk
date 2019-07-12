using Sample.Domain.Domain.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.Infrastructure.Data.EntityConfigurations
{
    public class EntityTypeAddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(p => p.City).IsRequired().HasMaxLength(100);
            builder.Property(p => p.ZipCode).IsRequired().HasMaxLength(10);
            builder.Property(p => p.Country).IsRequired().HasMaxLength(3);
            builder.Property(p => p.Street).IsRequired().HasMaxLength(500);
            builder.Property(p => p.State).IsRequired().HasMaxLength(50);
        }
    }
}
