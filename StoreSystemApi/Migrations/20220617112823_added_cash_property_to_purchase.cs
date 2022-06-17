using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreSystemApi.Migrations
{
    public partial class added_cash_property_to_purchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cash",
                table: "Purchases",
                type: "decimal(19,4)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cash",
                table: "Purchases");
        }
    }
}
