using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadByAdmin",
                table: "LoanDisbursements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByUser",
                table: "LoanDisbursements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByAdmin",
                table: "LoanApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByUser",
                table: "LoanApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByAdmin",
                table: "EligibilityChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadByUser",
                table: "EligibilityChecks",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsReadByAdmin", "IsReadByUser" },
                values: new object[] { false, true });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsReadByAdmin", "IsReadByUser" },
                values: new object[] { false, true });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsReadByAdmin", "IsReadByUser" },
                values: new object[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadByAdmin",
                table: "LoanDisbursements");

            migrationBuilder.DropColumn(
                name: "IsReadByUser",
                table: "LoanDisbursements");

            migrationBuilder.DropColumn(
                name: "IsReadByAdmin",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "IsReadByUser",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "IsReadByAdmin",
                table: "EligibilityChecks");

            migrationBuilder.DropColumn(
                name: "IsReadByUser",
                table: "EligibilityChecks");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "5f72e1c7-25f6-466e-b0da-da6f81928bb3", new DateTime(2026, 3, 20, 9, 48, 3, 845, DateTimeKind.Utc).AddTicks(9390), "6d38bf72-189d-4c6d-8ed3-38613ead894a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "1e2487fe-619e-4580-a70e-bcae7a8ddbed", new DateTime(2026, 3, 20, 9, 48, 3, 845, DateTimeKind.Utc).AddTicks(9510), "8b22f14a-d192-46f0-935a-e61ebbd23d50" });

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "Banks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 20, 9, 48, 3, 847, DateTimeKind.Utc).AddTicks(5010));
        }
    }
}
