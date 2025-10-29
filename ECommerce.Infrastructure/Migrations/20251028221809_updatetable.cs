using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "119b017f-2ffb-4450-a3df-285fc955f8a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c407f82d-6494-4810-87d9-8679458b7a7c");

            migrationBuilder.RenameColumn(
                name: "Nmae",
                table: "Products",
                newName: "Name");

            migrationBuilder.CreateTable(
                name: "CheckoutAchieves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedData = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutAchieves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "47839dd4-1542-4dac-beb4-bbd1198285bd", null, "Admin", "ADMIN" },
                    { "996a59c2-27c6-4434-98b9-140f300e520c", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("326d85ab-29d6-43b6-9bdb-e39cf9e77731"), "CreditCard" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckoutAchieves");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47839dd4-1542-4dac-beb4-bbd1198285bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "996a59c2-27c6-4434-98b9-140f300e520c");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Nmae");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "119b017f-2ffb-4450-a3df-285fc955f8a7", null, "User", "USER" },
                    { "c407f82d-6494-4810-87d9-8679458b7a7c", null, "Admin", "ADMIN" }
                });
        }
    }
}
