using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreSystemApi.Migrations
{
    public partial class removed_bonus_card_fk_from_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusCardId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BonusCardId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
