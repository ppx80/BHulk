﻿// <auto-generated />

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sample.Infrastructure.Data.Migrations
{
    [DbContext(typeof(OrdersDbContext))]
    partial class OrdersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConfigurationSample.Domain.Domain.Order.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OrderCode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ConfigurationSample.Domain.Domain.Order.Order", b =>
                {
                    b.OwnsOne("ConfigurationSample.Domain.Domain.Order.OrderDetails", "OrderDetails", b1 =>
                        {
                            b1.Property<int>("OrderId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<decimal>("TotalAmount")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.HasOne("ConfigurationSample.Domain.Domain.Order.Order")
                                .WithOne("OrderDetails")
                                .HasForeignKey("ConfigurationSample.Domain.Domain.Order.OrderDetails", "OrderId")
                                .OnDelete(DeleteBehavior.Cascade);

                            b1.OwnsOne("ConfigurationSample.Domain.Domain.Order.Address", "BillingAddress", b2 =>
                                {
                                    b2.Property<int>("OrderDetailsOrderId")
                                        .ValueGeneratedOnAdd()
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<string>("City")
                                        .IsRequired()
                                        .HasMaxLength(100);

                                    b2.Property<string>("Country")
                                        .IsRequired()
                                        .HasMaxLength(3);

                                    b2.Property<string>("State")
                                        .IsRequired()
                                        .HasMaxLength(50);

                                    b2.Property<string>("Street")
                                        .IsRequired()
                                        .HasMaxLength(500);

                                    b2.Property<string>("ZipCode")
                                        .IsRequired()
                                        .HasMaxLength(10);

                                    b2.HasKey("OrderDetailsOrderId");

                                    b2.ToTable("Orders");

                                    b2.HasOne("ConfigurationSample.Domain.Domain.Order.OrderDetails")
                                        .WithOne("BillingAddress")
                                        .HasForeignKey("ConfigurationSample.Domain.Domain.Order.Address", "OrderDetailsOrderId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });

                            b1.OwnsOne("ConfigurationSample.Domain.Domain.Order.Address", "ShippingAddress", b2 =>
                                {
                                    b2.Property<int>("OrderDetailsOrderId")
                                        .ValueGeneratedOnAdd()
                                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                                    b2.Property<string>("City");

                                    b2.Property<string>("Country");

                                    b2.Property<string>("State");

                                    b2.Property<string>("Street");

                                    b2.Property<string>("ZipCode");

                                    b2.HasKey("OrderDetailsOrderId");

                                    b2.ToTable("Orders");

                                    b2.HasOne("ConfigurationSample.Domain.Domain.Order.OrderDetails")
                                        .WithOne("ShippingAddress")
                                        .HasForeignKey("ConfigurationSample.Domain.Domain.Order.Address", "OrderDetailsOrderId")
                                        .OnDelete(DeleteBehavior.Cascade);
                                });
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
