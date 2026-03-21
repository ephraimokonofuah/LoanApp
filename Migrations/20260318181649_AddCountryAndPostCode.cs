using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryAndPostCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "Country", "CreatedAt", "PostCode", "SecurityStamp" },
                values: new object[] { "d1181bd2-73b4-4df7-93b0-f4a9d6a3dade", null, new DateTime(2026, 3, 18, 18, 16, 47, 354, DateTimeKind.Utc).AddTicks(8240), null, "0708a94d-4c20-4b22-8251-a2f37b923e94" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "Country", "CreatedAt", "PostCode", "SecurityStamp" },
                values: new object[] { "f8ba4487-9f27-4a59-aee5-ef3ea0fb30ba", null, new DateTime(2026, 3, 18, 18, 16, 47, 354, DateTimeKind.Utc).AddTicks(8390), null, "1a46a4e8-de20-47d4-ac89-b654aceb162c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "5f534199-52b6-44c0-bde9-8628795bf56c", new DateTime(2026, 3, 18, 16, 23, 38, 34, DateTimeKind.Utc).AddTicks(980), "ea2bea87-5868-4fd1-8935-a9ec7f9ac853" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "32c463b2-d1cf-4e2f-b99a-2afdfc63e9af", new DateTime(2026, 3, 18, 16, 23, 38, 34, DateTimeKind.Utc).AddTicks(1110), "ac92f52e-3413-4cd3-b851-f80352a8ba04" });
        }
    }
}
