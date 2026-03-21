using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "649fe4ea-e177-480a-883b-8adfeeaff029", new DateTime(2026, 3, 14, 9, 57, 9, 188, DateTimeKind.Utc).AddTicks(5590), "12e9be7c-2668-4f0d-8e91-5e8051a3269b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "33c16cd0-c087-46e5-b14d-90635235ba22", new DateTime(2026, 3, 14, 9, 57, 9, 188, DateTimeKind.Utc).AddTicks(5740), "4a9c6910-7e07-42ca-8613-2ea8184421e0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "39d6d2dd-f22c-4eb2-9312-6c660ab4ac70", new DateTime(2026, 3, 14, 9, 29, 23, 152, DateTimeKind.Utc).AddTicks(800), "0c2ec18f-da2c-49cf-bc75-3ae7352f0c93" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "15520818-30c4-4adb-9ed0-16549b625e15", new DateTime(2026, 3, 14, 9, 29, 23, 152, DateTimeKind.Utc).AddTicks(900), "8dc00615-1349-4be3-87ec-d9088c49a4c8" });
        }
    }
}
