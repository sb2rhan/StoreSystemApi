using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreSystemApi.Migrations
{
    public partial class added_purchase_and_product_props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Products",
                newName: "StockAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "Purchases",
                type: "decimal(19,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "StockAmount",
                table: "Products",
                newName: "Amount");
        }
    }
}
