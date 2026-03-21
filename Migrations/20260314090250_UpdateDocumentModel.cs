using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDocumentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Documents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewNotes",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "Documents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "d61b01c4-ab12-4c9e-8aa8-6b383f4f552d", new DateTime(2026, 3, 14, 9, 2, 49, 491, DateTimeKind.Utc).AddTicks(3350), "da46494c-a1d8-462a-9327-d02fa0406bf9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "7f5e295f-27d8-441a-9b50-c14dee921c51", new DateTime(2026, 3, 14, 9, 2, 49, 491, DateTimeKind.Utc).AddTicks(3490), "b4b6b484-9591-49ff-a47a-a54af563ed92" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ReviewNotes",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Documents");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "c260a60c-478f-40ad-ad19-f17650bf0c8e", new DateTime(2026, 3, 14, 1, 32, 30, 582, DateTimeKind.Utc).AddTicks(6050), "a67c73fe-bf37-43f8-bde9-e28e002a85b0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "a87d5d9f-d08d-4e03-81ca-2cced27f4dee", new DateTime(2026, 3, 14, 1, 32, 30, 582, DateTimeKind.Utc).AddTicks(6150), "286e1512-73ff-4c3d-a67d-237186325f40" });
        }
    }
}
