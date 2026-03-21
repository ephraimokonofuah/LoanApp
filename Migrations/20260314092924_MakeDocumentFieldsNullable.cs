using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeDocumentFieldsNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReviewNotes",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReviewNotes",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
