using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.Infrastructure.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderCode = table.Column<string>(maxLength: 10, nullable: false),
                    OrderDetails_TotalAmount = table.Column<decimal>(nullable: true),
                    OrderDetails_BillingAddress_Street = table.Column<string>(maxLength: 500, nullable: false),
                    OrderDetails_BillingAddress_City = table.Column<string>(maxLength: 100, nullable: false),
                    OrderDetails_BillingAddress_State = table.Column<string>(maxLength: 50, nullable: false),
                    OrderDetails_BillingAddress_Country = table.Column<string>(maxLength: 3, nullable: false),
                    OrderDetails_BillingAddress_ZipCode = table.Column<string>(maxLength: 10, nullable: false),
                    OrderDetails_ShippingAddress_Street = table.Column<string>(nullable: true),
                    OrderDetails_ShippingAddress_City = table.Column<string>(nullable: true),
                    OrderDetails_ShippingAddress_State = table.Column<string>(nullable: true),
                    OrderDetails_ShippingAddress_Country = table.Column<string>(nullable: true),
                    OrderDetails_ShippingAddress_ZipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id",
                table: "Orders",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
