using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedDataSecurityStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "83666dfa-19e9-4d07-9b99-8850f0bd49e3", new DateTime(2026, 3, 24, 13, 0, 37, 90, DateTimeKind.Utc).AddTicks(5820), "a1b2c3d4-e5f6-7890-abcd-ef1234567890" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "8fa7daa9-8a58-400d-9fb7-8e869cfc9bc4", new DateTime(2026, 3, 24, 13, 0, 37, 90, DateTimeKind.Utc).AddTicks(5930), "b2c3d4e5-f6a7-8901-bcde-f12345678901" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 0, 37, 91, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 0, 37, 91, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 0, 37, 91, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 0, 37, 91, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 0, 37, 91, DateTimeKind.Utc).AddTicks(7860));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "4cdb4c70-c311-4608-ab24-708a8aa55beb", new DateTime(2026, 3, 22, 21, 1, 47, 940, DateTimeKind.Utc).AddTicks(2310), "2e06508b-8c1b-4480-bb28-f9e2e9b60b18" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "1b362c3d-2fa6-40fe-9afe-d0c066985978", new DateTime(2026, 3, 22, 21, 1, 47, 940, DateTimeKind.Utc).AddTicks(2490), "44d3ef06-22a3-401f-9a4a-18bf89670388" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5820));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5830));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 21, 1, 47, 941, DateTimeKind.Utc).AddTicks(5830));
        }
    }
}
