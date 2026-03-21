using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentRequestNote",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DocumentsRequested",
                table: "LoanApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReviewedBy",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "46dd5f72-0997-4010-b06d-c5464b150f67", new DateTime(2026, 3, 18, 12, 48, 43, 697, DateTimeKind.Utc).AddTicks(5840), "fb625e62-147a-4a95-b606-d65837cadd60" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "fe6ed49d-9ddc-4bcd-9d89-c4cadf26933c", new DateTime(2026, 3, 18, 12, 48, 43, 697, DateTimeKind.Utc).AddTicks(5960), "aa258fbc-910d-4422-bea7-82a75d80ea16" });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DocumentRequestNote", "DocumentsRequested" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DocumentRequestNote", "DocumentsRequested" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DocumentRequestNote", "DocumentsRequested" },
                values: new object[] { null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentRequestNote",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "DocumentsRequested",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ReviewedBy",
                table: "Documents");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "8371c3c7-2fe3-4c5d-a76a-3914b436364a", new DateTime(2026, 3, 18, 12, 5, 58, 595, DateTimeKind.Utc).AddTicks(2000), "625ec1fd-c075-41e5-8d44-189417f5b5d8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "c96147a9-6a07-482b-b37c-f554c17e2f02", new DateTime(2026, 3, 18, 12, 5, 58, 595, DateTimeKind.Utc).AddTicks(2120), "cda36555-70be-43de-9188-50d17c23c11f" });
        }
    }
}
