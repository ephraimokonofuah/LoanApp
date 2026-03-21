using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanApp.Migrations
{
    /// <inheritdoc />
    public partial class LoanApplicationSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LoanApplications",
                columns: new[] { "Id", "CreatedAt", "DurationMonths", "InterestRate", "LoanAmount", "LoanPurpose", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 13, 20, 39, 23, 315, DateTimeKind.Utc).AddTicks(4440), 24, 5.5m, 5000m, "Home Improvement", "Pending" },
                    { 2, new DateTime(2026, 3, 13, 20, 39, 23, 315, DateTimeKind.Utc).AddTicks(4450), 36, 4.2m, 15000m, "Car Purchase", "Approved" },
                    { 3, new DateTime(2026, 3, 13, 20, 39, 23, 315, DateTimeKind.Utc).AddTicks(4450), 12, 6.0m, 3000m, "Medical Expenses", "Rejected" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LoanApplications",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
