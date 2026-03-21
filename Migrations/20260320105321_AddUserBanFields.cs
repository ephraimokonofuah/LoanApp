using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBanFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BanReason",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BannedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "BanReason", "BannedAt", "ConcurrencyStamp", "CreatedAt", "IsBanned", "SecurityStamp" },
                values: new object[] { null, null, "421b122e-0fc7-47f4-be71-efce1ee34d34", new DateTime(2026, 3, 20, 10, 53, 20, 680, DateTimeKind.Utc).AddTicks(3520), false, "27795422-2866-4afa-9947-79a2a5f491e2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "BanReason", "BannedAt", "ConcurrencyStamp", "CreatedAt", "IsBanned", "SecurityStamp" },
                values: new object[] { null, null, "1baee647-7010-4895-8694-66cfa71c6909", new DateTime(2026, 3, 20, 10, 53, 20, 680, DateTimeKind.Utc).AddTicks(3630), false, "4250c155-f7ef-4be4-93e6-ce5d19b19a2e" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 53, 20, 681, DateTimeKind.Utc).AddTicks(3880));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 53, 20, 681, DateTimeKind.Utc).AddTicks(3880));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 53, 20, 681, DateTimeKind.Utc).AddTicks(3890));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 53, 20, 681, DateTimeKind.Utc).AddTicks(3890));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 53, 20, 681, DateTimeKind.Utc).AddTicks(3890));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanReason",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BannedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "63c2bac1-8a68-4875-92e3-8d8794def14a", new DateTime(2026, 3, 20, 10, 33, 41, 364, DateTimeKind.Utc).AddTicks(4150), "0ac49b0a-27d5-452a-9ce8-85d277c43cd3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "05c4c679-10f9-49fb-94db-bb7a9f0ec54e", new DateTime(2026, 3, 20, 10, 33, 41, 364, DateTimeKind.Utc).AddTicks(4250), "8005b2a7-10a3-4d2a-a785-72fa6b607892" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 33, 41, 365, DateTimeKind.Utc).AddTicks(2510));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 33, 41, 365, DateTimeKind.Utc).AddTicks(2510));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 33, 41, 365, DateTimeKind.Utc).AddTicks(2520));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 33, 41, 365, DateTimeKind.Utc).AddTicks(2520));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 10, 33, 41, 365, DateTimeKind.Utc).AddTicks(2520));
        }
    }
}
