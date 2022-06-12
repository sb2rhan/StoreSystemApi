using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreSystemApi.Migrations
{
    public partial class admin_user_seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"), "46477ba0-29d2-4ba5-b10c-471a6aa88869", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BankCard", "BirthDate", "BonusCardId", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "MiddleName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"), 0, null, null, null, "598b1fc2-783d-4c22-82e3-0fcc51bc4f16", "admin@mail.com", true, "Admin", "Admin", false, null, "Admin", "ADMIN@MAIL.COM", "ADMIN", "AQAAAAEAACcQAAAAEFvpwHR0kH1sy6DQWpIndLCdmZsahrddpi9XzA5DkTSadoKfzl+amp9ya+lWuMpIWQ==", "+111111111111", true, null, false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"), new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"), new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2c5e174e-3b0e-446f-86af-483d56fd7210"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8e445865-a24d-4543-a6c6-9443d048cdb9"));
        }
    }
}
