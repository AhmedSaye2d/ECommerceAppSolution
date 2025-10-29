using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "42a404a7-6144-4dca-80e6-2710b2fc0bc9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c8142516-64c8-4ad8-959d-8b0d57876999");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "119b017f-2ffb-4450-a3df-285fc955f8a7", null, "User", "USER" },
                    { "c407f82d-6494-4810-87d9-8679458b7a7c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "119b017f-2ffb-4450-a3df-285fc955f8a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c407f82d-6494-4810-87d9-8679458b7a7c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "42a404a7-6144-4dca-80e6-2710b2fc0bc9", null, "User", "USER" },
                    { "c8142516-64c8-4ad8-959d-8b0d57876999", null, "Admin", "ADMIN" }
                });
        }
    }
}
