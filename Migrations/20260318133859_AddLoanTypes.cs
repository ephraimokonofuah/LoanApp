using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLoanTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanTypeId",
                table: "LoanApplications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoanTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-1",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "4e4fa387-de04-4a4b-a712-6808afcbc035", new DateTime(2026, 3, 18, 13, 38, 58, 462, DateTimeKind.Utc).AddTicks(5630), "9f46c875-f1ce-4cf7-8c0c-009c729c381b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "seed-user-2",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "SecurityStamp" },
                values: new object[] { "0e38a60e-9b71-42d8-83c6-463f333b7b2a", new DateTime(2026, 3, 18, 13, 38, 58, 462, DateTimeKind.Utc).AddTicks(5740), "38e29280-1f5b-48a7-9280-5c30c3bc379a" });

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 1,
                column: "LoanTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 2,
                column: "LoanTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 3,
                column: "LoanTypeId",
                value: null);

            migrationBuilder.InsertData(
                table: "LoanTypes",
                columns: new[] { "Id", "Description", "InterestRate", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "For personal expenses such as medical bills, vacations, or emergencies.", 12.0m, true, "Personal Loan" },
                    { 2, "For starting or expanding a business.", 8.0m, true, "Business Loan" },
                    { 3, "For tuition fees, books, and other educational expenses.", 5.5m, true, "Education Loan" },
                    { 4, "For purchasing or refinancing a home.", 3.5m, true, "Mortgage Loan" },
                    { 5, "For purchasing a new or used vehicle.", 6.5m, true, "Auto Loan" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_LoanTypeId",
                table: "LoanApplications",
                column: "LoanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_LoanTypes_LoanTypeId",
                table: "LoanApplications",
                column: "LoanTypeId",
                principalTable: "LoanTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_LoanTypes_LoanTypeId",
                table: "LoanApplications");

            migrationBuilder.DropTable(
                name: "LoanTypes");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_LoanTypeId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "LoanTypeId",
                table: "LoanApplications");

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
        }
    }
}
