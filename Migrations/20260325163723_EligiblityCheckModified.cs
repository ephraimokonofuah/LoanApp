using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class EligiblityCheckModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "4bde3b64-8417-4332-a4a7-b509a87bf14f", new DateTime(2026, 3, 25, 16, 37, 22, 572, DateTimeKind.Utc).AddTicks(790) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "37293bff-53b5-4f4a-b857-dfe9a6af7b51", new DateTime(2026, 3, 25, 16, 37, 22, 572, DateTimeKind.Utc).AddTicks(900) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 16, 37, 22, 573, DateTimeKind.Utc).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 16, 37, 22, 573, DateTimeKind.Utc).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 16, 37, 22, 573, DateTimeKind.Utc).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 16, 37, 22, 573, DateTimeKind.Utc).AddTicks(1170));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 16, 37, 22, 573, DateTimeKind.Utc).AddTicks(1170));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "785ed437-0fc8-46e6-99a7-b59c7f29b878", new DateTime(2026, 3, 24, 13, 55, 27, 107, DateTimeKind.Utc).AddTicks(3220) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt" },
                values: new object[] { "33a8071e-81cf-4c14-828b-5a3f5791ec95", new DateTime(2026, 3, 24, 13, 55, 27, 107, DateTimeKind.Utc).AddTicks(3330) });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 55, 27, 108, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 55, 27, 108, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 55, 27, 108, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 55, 27, 108, DateTimeKind.Utc).AddTicks(4130));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 24, 13, 55, 27, 108, DateTimeKind.Utc).AddTicks(4130));
        }
    }
}
